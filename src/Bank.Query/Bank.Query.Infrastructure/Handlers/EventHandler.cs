namespace Bank.Query.Infrastructure.Handlers
{
	using System.Threading.Tasks;

	using Bank.Common.Events;
	using Bank.Query.Domain.Anemics;
	using Bank.Query.Domain.Repositories;

	/**
	 * This class is responsible for handling events that are raised by the application.
	 * Once the EventConsumer consumes an event,
	 * it will invoke the relevent handler (.On()) method which will use the event message to build 
	 * the WithdrawalAnemic and insert the related record in the read database 
	 */
	public class EventHandler : IEventHandler
	{
		private readonly IWithdrawalRepository withdrawalRepository;
		private readonly IBankAccountRepository bankAccountRepository;
		public EventHandler(IWithdrawalRepository withdrawalRepository, IBankAccountRepository bankAccountRepository)
		{
			this.withdrawalRepository = withdrawalRepository;
			this.bankAccountRepository = bankAccountRepository;
		}
		public async Task On(WithdrawalEvent theEvent)
		{
			var account = await this.bankAccountRepository.GetByIdAsync(theEvent.AccountId);

			var balance = 0m;

			if (account != null)
			{
				balance = account.Balance;
			}

			// if balance is less than amount produce message, 
			if (balance < theEvent.Amount)
			{ 
				// publish insufficent balance message to messagebus. Another service can consume this message and notify the user
				// throw new Exception("Insufficient balance");
			}


			balance -= theEvent.Amount;

			account.Balance = balance;
			await this.bankAccountRepository.UpdateAsync(account);

			// publish insufficent balance message to messagebus. Another service can consume this message and notify the user

			var withdrawalAnemic = new WithdrawalAnemic
			{
				Id = theEvent.Id,
				AccountId = theEvent.AccountId,
				Amount = theEvent.Amount,
				TransactionDate = theEvent.TransactionDate
			};

			await this.withdrawalRepository.CreateAsync(withdrawalAnemic);
		}
	}
}
