namespace Bank.Command.Infrastructure.Handlers
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Options;

	using CQRS.Core.Domain;
	using CQRS.Core.Handlers;
	using CQRS.Core.Producers;
	using CQRS.Core.Infrastructure;
	using Bank.Command.Domain.Aggregates;
	using Bank.Command.Infrastructure.Config;

	public class EventSourcingHandler : IEventSourcingHandler<BankAccountAggregate>
	{
		private readonly IEventStore eventStore;
		private readonly MessageBusConfig messageBusConfig;
		private readonly IEventProducer _eventProducer;
		public EventSourcingHandler(IEventStore eventStore, IOptions<MessageBusConfig> messageBusConfig, IEventProducer eventProducer)
		{
			this.eventStore = eventStore;
			this.messageBusConfig = messageBusConfig.Value;
			this._eventProducer = eventProducer;
		}
		public async Task<BankAccountAggregate> GetByIdAsync(Guid aggregateId)
		{
			var aggregate = new BankAccountAggregate();
			var events = await eventStore.GetEventsAsync(aggregateId);

			if (events == null || !events.Any())
			{
				return aggregate;
			}
			// recrerate latest state of aggregate
			aggregate.ReplayEvents(events);
			var latestVersion = events.Select(x => x.Version).Max();
			aggregate.Version = latestVersion;

			return aggregate;

		}

		public async Task RepublishEventsAsync()
		{
			var aggregateIds = await eventStore.GetAggregateIdsAsync();

			if (aggregateIds == null || !aggregateIds.Any())
			{
				return;
			}

			foreach (var aggregateId in aggregateIds)
			{
				var aggregate = await GetByIdAsync(aggregateId);

				if (aggregate == null || !aggregate.Active)
				{
					continue;
				}

				var events = await eventStore.GetEventsAsync(aggregateId);
				//TODO: Review @evet
				foreach (var @event in events)
				{
					var topic = this.messageBusConfig.Topic;
					await _eventProducer.ProduceAsync(topic, @event);
				}
			}
		}

		public async Task SaveAsync(AggregateRoot aggregate)
		{
			await eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
			aggregate.MarkChangesAsCommitted();
		}
	}
}
