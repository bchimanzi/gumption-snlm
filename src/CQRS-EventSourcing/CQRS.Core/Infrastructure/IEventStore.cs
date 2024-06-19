namespace CQRS.Core.Infrastructure
{
	using System;
	using System.Threading.Tasks;
	using System.Collections.Generic;

	using CQRS.Core.Events;

	public interface IEventStore
	{
		Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
		Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);
		Task<bool> EventExistsAsync(Guid aggregateId);
		Task<List<Guid>> GetAggregateIdsAsync();
	}
}
