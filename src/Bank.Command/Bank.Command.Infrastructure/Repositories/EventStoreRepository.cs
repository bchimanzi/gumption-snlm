namespace Bank.Command.Infrastructure.Repositories
{
	using System;
	using System.Threading.Tasks;
	using System.Collections.Generic;
	using Microsoft.Extensions.Options;

	using MongoDB.Driver;

	using CQRS.Core.Domain;
	using CQRS.Core.Events;
	using Bank.Command.Infrastructure.Config;

	public class EventStoreRepository : IEventStoreRepository
	{
		private readonly IMongoCollection<EventModel> _eventStoreCollection;
		public EventStoreRepository(IOptions<MongoDbConfig> config)
		{
			var mogoClient = new MongoClient(config.Value.ConnectionString);
			var mongoDatabase = mogoClient.GetDatabase(config.Value.Database);
			_eventStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Value.Collection);
		}
		public async Task SaveAsync(EventModel @event)
		{
			/*adding .ConfigureAwait(false) avoids callback to be invocked on the original context
			*improves performance & prevents deadlocks
			*/
			await _eventStoreCollection.InsertOneAsync(@event).ConfigureAwait(false);
		}

		public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
		{
			return await _eventStoreCollection.Find(x => x.AggregateIdentifier == aggregateId).ToListAsync().ConfigureAwait(false);
		}

		public async Task<List<EventModel>> FindAllAsync()
		{
			return await _eventStoreCollection.Find(_ => true).ToListAsync().ConfigureAwait(false);
		}
	}
}
