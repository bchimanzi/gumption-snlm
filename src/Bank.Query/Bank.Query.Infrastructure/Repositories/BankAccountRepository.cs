namespace Bank.Query.Infrastructure.Repositories
{
	using System;
	using System.Threading.Tasks;
	using System.Collections.Generic;
	using Microsoft.EntityFrameworkCore;

	using Bank.Query.Domain.Anemics;
	using Bank.Query.Domain.Repositories;
	using Bank.Query.Infrastructure.DataAccess;


	public class BankAccountRepository : IBankAccountRepository
	{
		private readonly DatabaseContextFactory contextFactory;

		public BankAccountRepository(DatabaseContextFactory contextFactory)
		{
			this.contextFactory = contextFactory;
		}
		public async Task<BankAccountAnemic> GetByIdAsync(Guid id)
		{
			using DatabaseContext context = this.contextFactory.CreateDbContext();
			return await context.BankAccounts.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<List<BankAccountAnemic>> ListAllAsync()
		{
			using DatabaseContext context = this.contextFactory.CreateDbContext();
			return await context.BankAccounts.ToListAsync();
		}

		public async Task UpdateAsync(BankAccountAnemic bankAccount)
		{
			using DatabaseContext context = this.contextFactory.CreateDbContext();
			context.BankAccounts.Update(bankAccount);

			_ = await context.SaveChangesAsync();
		}
	}
}
