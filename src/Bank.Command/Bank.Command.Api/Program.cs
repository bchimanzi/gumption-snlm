using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Serilog;
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

var builder = WebApplication.CreateBuilder(args);

/**
 * MongoDb in its default state does not support polymorphism.
 * You set the Bson class to tell it that all the concrete event object types extends the base event class
 * 
 */
BsonClassMap.RegisterClassMap<BaseEvent>();
BsonClassMap.RegisterClassMap<WithdrawalEvent>();

// Add services to the container.

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
		.WriteTo.Console()
		.ReadFrom.Configuration(context.Configuration));

builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
builder.Services.Configure<MessageBusConfig>(builder.Configuration.GetSection(nameof(MessageBusConfig)));

builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventProducer, KafkaEventProducer>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<WithdrawalAggregate>, EventSourcingHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();

var commandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var dispatcher = new CommandDispatcher();
dispatcher.RegisterHandler<WithdrawalCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<RestoreReadDatabaseCommand>(commandHandler.HandleAsync);
builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
