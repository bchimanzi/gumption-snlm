namespace Bank.Query.Api.Queries
{
	using System.Threading.Tasks;
	using System.Collections.Generic;

	using Bank.Query.Domain.Anemics;

	public interface IQueryHandler
	{
		//Task<List<BankAccountAnemic>> HandleAsync(FindAllAccountsQuery query);
		//Task<List<BankAccountAnemic>> HandleAsync(FindAccountByIdQuery query);
		Task<List<WithdrawalAnemic>> HandleAsync(FindWithdrawalsByAccountIdQuery query);
		Task<List<WithdrawalAnemic>> HandleAsync(FindWithdrawalByIdQuery query);
	}
}
