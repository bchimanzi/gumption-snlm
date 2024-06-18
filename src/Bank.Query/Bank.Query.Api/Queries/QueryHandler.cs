namespace Bank.Query.Api.Queries
{
	using System.Threading.Tasks;
	using System.Collections.Generic;

	using Bank.Query.Domain.Anemics;
	using Bank.Query.Domain.Repositories;

	public class QueryHandler : IQueryHandler
	{
		private readonly IWithdrawalRepository withdrawalRepository;

        public QueryHandler(IWithdrawalRepository withdrawalRepository)
        {
            this.withdrawalRepository = withdrawalRepository;
        }

  //      public Task<List<BankAccountAnemic>> HandleAsync(FindAllAccountsQuery query)
		//{
		//	throw new System.NotImplementedException();
		//}

		//public Task<List<BankAccountAnemic>> HandleAsync(FindAccountByIdQuery query)
		//{
		//	throw new System.NotImplementedException();
		//}

		public async Task<List<WithdrawalAnemic>> HandleAsync(FindWithdrawalsByAccountIdQuery query)
		{
			return await withdrawalRepository.ListByAccountIdAsync(query.AccountId);
		}

		public async Task<List<WithdrawalAnemic>> HandleAsync(FindWithdrawalByIdQuery query)
		{
			var withdrawals =  await withdrawalRepository.GetByIdAsync(query.Id);

			return new List<WithdrawalAnemic> { withdrawals };
		}
	}
}
