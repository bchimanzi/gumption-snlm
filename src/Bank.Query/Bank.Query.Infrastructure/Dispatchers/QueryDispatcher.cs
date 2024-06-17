namespace Bank.Query.Infrastructure.Dispatchers
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	
	using CQRS.Core.Queries;
	using CQRS.Core.Infrastructure;
	using Bank.Query.Domain.Anemics;

	public class QueryDispatcher : IQueryDispatcher<WithdrawalAnemic>
	{
		private readonly Dictionary<Type, Func<BaseQuery, Task<List<WithdrawalAnemic>>>> _handlers = new();

		public void RegisterHandler<TQuery>(Func<TQuery, Task<List<WithdrawalAnemic>>> handler) where TQuery : BaseQuery
		{
			if (_handlers.ContainsKey(typeof(TQuery)))
			{
				throw new IndexOutOfRangeException("You cannot register the same query handler twice!");
			}

			_handlers.Add(typeof(TQuery), x => handler((TQuery)x));
		}

		public async Task<List<WithdrawalAnemic>> SendAsync(BaseQuery query)
		{
			if (_handlers.TryGetValue(query.GetType(), out Func<BaseQuery, Task<List<WithdrawalAnemic>>> handler))
			{
				return await handler(query);
			}

			throw new ArgumentNullException(nameof(handler), "No query handler was registered!");
		}
	}
}
