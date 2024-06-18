namespace Bank.Command.Infrastructure.Stores
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Collections.Generic;
	using Microsoft.Extensions.Options;

	using CQRS.Core.Domain;
	using CQRS.Core.Events;
	using CQRS.Core.Producers;
	using CQRS.Core.Exceptions;
	using CQRS.Core.Infrastructure;
	using Bank.Command.Domain.Aggregates;
	using Bank.Command.Infrastructure.Config;

	public class EventStore : IEventStore
	{
		private readonly IEventStoreRepository _eventStoreRepository;
		private readonly IEventProducer eventProducer;
		private readonly MessageBusConfig messageBusConfig;

		public EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer, IOptions<MessageBusConfig> messageBusConfig)
		{
			this._eventStoreRepository = eventStoreRepository;
			this.eventProducer = eventProducer;
			this.messageBusConfig = messageBusConfig.Value;
		}

		public async Task<bool> EventExistsAsync(Guid aggregateId)
		{
			var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

			return eventStream != null && eventStream.Any();
		}

		public async Task<List<Guid>> GetAggregateIdsAsync()
		{
			var eventStream = await _eventStoreRepository.FindAllAsync();

			if (eventStream == null || !eventStream.Any())
			{
				throw new ArgumentNullException(nameof(eventStream), "Could not retrieve event stream from the event store!");
			}

			return eventStream.Select(x => x.AggregateIdentifier).Distinct().ToList();
		}

		public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
		{
			var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

			if (eventStream == null || !eventStream.Any())
			{
				throw new AggregateNotFoundException($"Incorrect Aggregate ID provided!");
			}

			return eventStream.OrderBy(x => x.Version).Select(x => x.EventData).ToList();
		}

		public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
		{
			var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
			//^1 = length - 1
			if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
			{
				throw new ConcurrencyException();
			}

			var version = expectedVersion;

			foreach (var theEvent in events)
			{
				version++;
				theEvent.Version = version;
				var eventType = theEvent.GetType().Name;// name of concrete event type
				var eventModel = new EventModel
				{
					TimeStamp = DateTime.UtcNow,
					AggregateIdentifier = aggregateId,
					AggregateType = nameof(WithdrawalAggregate),
					Version = version,
					EventType = eventType,
					EventData = theEvent
				};

				/*
				 * Ideally a transaction should be used here to ensure that the event is saved to the event store and produced to the message bus
				 * But since MongoDB does not support transactions on standalone instances, i excluded it
				 */		
				await _eventStoreRepository.SaveAsync(eventModel);

				await this.eventProducer.ProduceAsync(this.messageBusConfig.Topic, theEvent);
			}
		}
	}
}
