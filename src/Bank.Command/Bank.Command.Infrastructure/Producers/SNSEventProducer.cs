namespace Bank.Command.Infrastructure.Producers
{

	using System;
	using System.Threading.Tasks;

	using CQRS.Core.Events;
	using CQRS.Core.Producers;

	//TODO: Implement
	public class SNSEventProducer : IEventProducer
	{
		public Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent
		{
			throw new NotImplementedException();
		}
	}
}
