namespace CQRS.Core.Domain
{
	using System;
	using System.Threading.Tasks;
	using System.Collections.Generic;

	using CQRS.Core.Events;

	public interface IEventStoreRepository
	{
		Task SaveAsync(EventModel @event);
		Task<List<EventModel>> FindByAggregateId(Guid aggregateId);

		Task<List<EventModel>> FindAllAsync();

	}
}
