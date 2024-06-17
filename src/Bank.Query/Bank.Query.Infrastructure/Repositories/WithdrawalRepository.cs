namespace Bank.Query.Infrastructure.Repositories
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Collections.Generic;
	using Microsoft.EntityFrameworkCore;

	using Bank.Query.Domain.Anemics;
	using Bank.Query.Domain.Repositories;
	using Bank.Query.Infrastructure.DataAccess;

	public class WithdrawalRepository : IWithdrawalRepository
	{
		private readonly DatabaseContextFactory contextFactory;
		public WithdrawalRepository(DatabaseContextFactory contextFactory)
		{
			this.contextFactory = contextFactory;
		}

		public async Task CreateAsync(WithdrawalAnemic withdrawal)
		{
			using DatabaseContext context = this.contextFactory.CreateDbContext();
			context.Withdrawals.Add(withdrawal);

			_ = await context.SaveChangesAsync();
		}

		public async Task<WithdrawalAnemic> GetByIdAsync(Guid withdrawalId)
		{
			using DatabaseContext context = this.contextFactory.CreateDbContext();

			return await context.Withdrawals
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == withdrawalId);
		}

		public async Task<List<WithdrawalAnemic>> ListByAccountIdAsync(Guid accountId)
		{
			using DatabaseContext context = this.contextFactory.CreateDbContext();

			return await context.Withdrawals
			.AsNoTracking()
			.Where(x => x.AccountId == accountId).ToListAsync();
		}
	}
}
