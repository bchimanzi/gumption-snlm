namespace CQRS.Core.Producers
{
	using System.Threading.Tasks;

	using CQRS.Core.Events;

	public interface IEventProducer
	{
		Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent;
	}
}
