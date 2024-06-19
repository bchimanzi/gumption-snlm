namespace Bank.Query.Api.Queries
{
	using System.Threading.Tasks;
	using System.Collections.Generic;

	using Bank.Query.Domain.Anemics;
	using Bank.Query.Domain.Repositories;

	public class QueryHandler : IQueryHandler
	{
		private readonly IWithdrawalRepository withdrawalRepository;
		private readonly IBankAccountRepository bankAccountRepository;

		public QueryHandler(IWithdrawalRepository withdrawalRepository, IBankAccountRepository bankAccountRepository)
		{
			this.withdrawalRepository = withdrawalRepository;
			this.bankAccountRepository = bankAccountRepository;
		}

		public async Task<List<WithdrawalAnemic>> HandleAsync(FindWithdrawalsByAccountIdQuery query)
		{
			return await withdrawalRepository.ListByAccountIdAsync(query.AccountId);
		}

		public async Task<List<WithdrawalAnemic>> HandleAsync(FindWithdrawalByIdQuery query)
		{
			var withdrawals = await withdrawalRepository.GetByIdAsync(query.Id);

			return new List<WithdrawalAnemic> { withdrawals };
		}

		public async Task<List<BankAccountAnemic>> HandleAsync(FindAllAccountsQuery query)
		{
			return await bankAccountRepository.ListAllAsync();
		}
	}
}
