namespace Bank.Query.Domain.Repositories
{
	using System;
	using System.Threading.Tasks;
	using System.Collections.Generic;

	using Bank.Query.Domain.Anemics;

	public interface IWithdrawalRepository
	{
		Task CreateAsync(WithdrawalAnemic withdrawal);
		Task<WithdrawalAnemic> GetByIdAsync(Guid withdrawalId);
		Task<List<WithdrawalAnemic>> ListByAccountIdAsync(Guid accountId);
	}
}
