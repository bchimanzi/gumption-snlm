namespace Bank.Query.Api.Queries
{
	using CQRS.Core.Queries;
	using System;

	public class FindWithdrawalByIdQuery : BaseQuery
	{
		public Guid Id { get; set; }
	}
}
