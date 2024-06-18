namespace Bank.Query.Api.Queries
{
	using CQRS.Core.Queries;
	using System;

	public class FindAccountByIdQuery : BaseQuery
	{
		public Guid Id { get; set; }
	}
}
