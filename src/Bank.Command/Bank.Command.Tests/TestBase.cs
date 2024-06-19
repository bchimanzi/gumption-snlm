namespace Bank.Command.Tests
{
	using System;
	using System.IO;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;

	using Confluent.Kafka;
	using MongoDB.Bson.Serialization;

	using CQRS.Core.Domain;
	using CQRS.Core.Events;
	using CQRS.Core.Handlers;
	using Bank.Common.Events;
	using CQRS.Core.Producers;
	using CQRS.Core.Infrastructure;
	using Bank.Command.Api.Commands;
	using Bank.Command.Domain.Aggregates;
	using Bank.Command.Infrastructure.Config;
	using Bank.Command.Infrastructure.Stores;
	using Bank.Command.Infrastructure.Handlers;
	using Bank.Command.Infrastructure.Producers;
	using Bank.Command.Infrastructure.Dispatchers;
	using Bank.Command.Infrastructure.Repositories;

	[TestClass]
	public class TestBase
	{
		private readonly IConfiguration configuration;
		private readonly IServiceProvider serviceProvider;
		protected readonly ICommandDispatcher commandDispatcher;
		protected readonly IEventStore eventStore;

		public TestBase()
		{
			var serviceCollection = new ServiceCollection();

			this.configuration = this.BuildConfiguration(serviceCollection: serviceCollection);
			this.serviceProvider = this.ConfigureServices(serviceCollection: serviceCollection);
			this.commandDispatcher = this.GetDispatcher();
			this.eventStore = this.GetRequiredService<IEventStore>();
		}


		private IConfigurationRoot BuildConfiguration(ServiceCollection serviceCollection)
		{
			var configuration = new ConfigurationBuilder()
					.SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
					.AddJsonFile("appsettings.json", false)
					.Build();

			serviceCollection.AddSingleton<IConfiguration>(configuration);


			return configuration;
		}

		private IServiceProvider ConfigureServices(ServiceCollection serviceCollection)
		{

			BsonClassMap.RegisterClassMap<BaseEvent>();
			BsonClassMap.RegisterClassMap<WithdrawalEvent>();

			// Add services to the container.
			serviceCollection.Configure<MongoDbConfig>(configuration.GetSection(nameof(MongoDbConfig)));
			serviceCollection.Configure<ProducerConfig>(configuration.GetSection(nameof(ProducerConfig)));
			serviceCollection.Configure<MessageBusConfig>(configuration.GetSection(nameof(MessageBusConfig)));

			serviceCollection.AddScoped<IEventStoreRepository, EventStoreRepository>();
			serviceCollection.AddScoped<IEventProducer, KafkaEventProducer>();
			serviceCollection.AddScoped<IEventStore, EventStore>();
			serviceCollection.AddScoped<IEventSourcingHandler<WithdrawalAggregate>, EventSourcingHandler>();
			serviceCollection.AddScoped<ICommandHandler, CommandHandler>();

			var commandHandler = serviceCollection.BuildServiceProvider().GetRequiredService<ICommandHandler>();
			var dispatcher = new CommandDispatcher();
			dispatcher.RegisterHandler<WithdrawalCommand>(commandHandler.HandleAsync);

			serviceCollection.AddSingleton<ICommandDispatcher>(_ => dispatcher);

			serviceCollection.AddLogging(options =>
			{
				options.AddConsole();
				options.AddDebug();
			});


			return serviceCollection.BuildServiceProvider();
		}

		private ICommandDispatcher GetDispatcher()
		{
			return this.GetRequiredService<ICommandDispatcher>();
		}

		public T GetRequiredService<T>()
		{
			return this.serviceProvider.GetRequiredService<T>();
		}
	}
}
