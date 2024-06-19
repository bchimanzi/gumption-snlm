namespace CQRS.Core.Infrastructure
{
	using System;
	using System.Threading.Tasks;
	using System.Collections.Generic;

	using CQRS.Core.Queries;

	public interface IQueryDispatcher<TEntity>
	{
		void RegisterHandler<TQuery>(Func<TQuery, Task<List<TEntity>>> handler) where TQuery : BaseQuery;
		Task<List<TEntity>> SendAsync(BaseQuery query);
	}
}
