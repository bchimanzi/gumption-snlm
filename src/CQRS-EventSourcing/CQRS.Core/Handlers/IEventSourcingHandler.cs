namespace CQRS.Core.Handlers
{
	using System;
	using System.Threading.Tasks;

	using CQRS.Core.Domain;

	public interface IEventSourcingHandler<T>
	{
		Task SaveAsync(AggregateRoot aggregate);
		Task<T> GetByIdAsync(Guid aggregateId);

		Task RepublishEventsAsync();
	}
}
