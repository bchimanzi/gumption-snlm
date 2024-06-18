using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Confluent.Kafka;

using CQRS.Core.Consumers;
using Bank.Query.Api.Queries;
using CQRS.Core.Infrastructure;
using Bank.Query.Domain.Anemics;
using Bank.Query.Domain.Repositories;
using Bank.Query.Infrastructure.Config;
using Bank.Query.Infrastructure.Handlers;
using Bank.Query.Infrastructure.Consumers;
using Bank.Query.Infrastructure.DataAccess;
using Bank.Query.Infrastructure.Dispatchers;
using Bank.Query.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
Action<DbContextOptionsBuilder> configureDbContext = o => o.UseLazyLoadingProxies().UseNpgsql(builder.Configuration.GetConnectionString("Database"));

builder.Services.AddDbContext<DatabaseContext>(configureDbContext);
builder.Services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDbContext));

// create database and tables
var dataContext = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
dataContext.Database.EnsureCreated();

builder.Services.AddScoped<IWithdrawalRepository, WithdrawalRepository>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<IQueryHandler, QueryHandler>();
builder.Services.AddScoped<IEventHandler,Bank.Query.Infrastructure.Handlers.EventHandler>();
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.Configure<MessageBusConfig>(builder.Configuration.GetSection(nameof(MessageBusConfig)));
builder.Services.AddScoped<IEventConsumer, EventConsumer>();

// register query handler methods
var queryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IQueryHandler>();
var dispatcher = new QueryDispatcher();
dispatcher.RegisterHandler<FindWithdrawalsByAccountIdQuery>(queryHandler.HandleAsync);
dispatcher.RegisterHandler<FindWithdrawalByIdQuery>(queryHandler.HandleAsync);

builder.Services.AddSingleton<IQueryDispatcher<WithdrawalAnemic>>(_ => dispatcher);

builder.Services.AddControllers();
builder.Services.AddHostedService<ConsumerHostedService>();
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
