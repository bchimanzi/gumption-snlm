namespace Bank.Command.Infrastructure.Producers
{
	using System;
	using System.Text.Json;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Options;

	using Confluent.Kafka;

	using CQRS.Core.Events;
	using CQRS.Core.Producers;

	public class KafkaEventProducer : IEventProducer
	{
		private readonly ProducerConfig producerConfig;

		public KafkaEventProducer(IOptions<ProducerConfig> producerConfig)
		{
			this.producerConfig = producerConfig.Value;
		}

		public async Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent
		{
			using var producer = new ProducerBuilder<string, string>(producerConfig)
			.SetKeySerializer(Serializers.Utf8)
			.SetValueSerializer(Serializers.Utf8)
			.Build();

			var eventMessage = new Message<string, string>
			{
				Key = Guid.NewGuid().ToString(),
				Value = JsonSerializer.Serialize(@event, @event.GetType())
			};

			var deliveryResult = await producer.ProduceAsync(topic, eventMessage);

			if (deliveryResult.Status == PersistenceStatus.NotPersisted)
			{
				throw new Exception($"Could not produce event {@event.GetType().Name} message to topic - {topic} due to the following reason: {deliveryResult.Message}.");
			}
		}
	}
}
