namespace Bank.Query.Infrastructure.Consumers
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Options;
	using Microsoft.Extensions.DependencyInjection;
	
	using CQRS.Core.Consumers;
	using Bank.Query.Infrastructure.Config;

	public class ConsumerHostedService : IHostedService
	{
		private readonly ILogger<ConsumerHostedService> _logger;
		private readonly IServiceProvider _serviceProvider;
		private readonly MessageBusConfig messageBusConfig;

		public ConsumerHostedService(ILogger<ConsumerHostedService> logger, IServiceProvider serviceProvider, IOptions<MessageBusConfig> messageBusConfig)
		{
			_logger = logger;
			_serviceProvider = serviceProvider;
			this.messageBusConfig = messageBusConfig.Value;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Event Consumer Service running.");

			using (IServiceScope scope = _serviceProvider.CreateScope())
			{
				var topic = this.messageBusConfig.Topic;
				var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();

				Task.Run(() => eventConsumer.Consume(topic), cancellationToken);
			}

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Event Consumer Service Stopped");

			return Task.CompletedTask;
		}
	}
}
