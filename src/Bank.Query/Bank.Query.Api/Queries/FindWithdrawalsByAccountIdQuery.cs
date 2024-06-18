namespace Bank.Query.Api.Queries
{
	using System;

	using CQRS.Core.Queries;

	public class FindWithdrawalsByAccountIdQuery : BaseQuery
	{
        public Guid AccountId { get; set; }
    }
}
