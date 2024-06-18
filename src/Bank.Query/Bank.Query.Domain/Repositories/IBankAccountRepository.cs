namespace Bank.Query.Domain.Repositories
{
	using System;
	using System.Threading.Tasks;
	using System.Collections.Generic;

	using Bank.Query.Domain.Anemics;

	public interface IBankAccountRepository
	{
		Task<BankAccountAnemic> GetByIdAsync(Guid id);
		Task<List<BankAccountAnemic>> ListAllAsync();
		Task UpdateAsync(BankAccountAnemic bankAccount);
	}
}
