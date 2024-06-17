namespace Bank.Query.Infrastructure.DataAccess
{
	using Bank.Query.Domain.Anemics;
	using Microsoft.EntityFrameworkCore;

	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<BankAccountAnemic> BankAccounts { get; set; }
		public DbSet<WithdrawalAnemic> Withdrawals { get; set; }
	}
}
