namespace Bank.Query.Infrastructure.DataAccess
{
	using System;
	using Microsoft.EntityFrameworkCore;

	/// <summary>
	/// //// Factory for creating dbcontext instances 
	/// </summary>
	public class DatabaseContextFactory
	{
		private readonly Action<DbContextOptionsBuilder> _configureDbContext;

		public DatabaseContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
		{
			_configureDbContext = configureDbContext;
		}

		public DatabaseContext CreateDbContext()
		{
			DbContextOptionsBuilder<DatabaseContext> optionsBuilder = new();
			_configureDbContext(optionsBuilder);

			return new DatabaseContext(optionsBuilder.Options);
		}
	}
}
