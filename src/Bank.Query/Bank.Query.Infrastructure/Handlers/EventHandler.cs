namespace Bank.Query.Infrastructure.Handlers
{
	using System;
	using System.Threading.Tasks;

	using Bank.Common.Events;
	using Bank.Query.Domain.Anemics;
	using Bank.Query.Domain.Repositories;

	/**
	 * This class is responsible for handling events that are raised by the application.
	 * Once the EventConsumer consumes an event,
	 * it will invoke the relevent handler (.On()) method which will use the event message to build 
	 * the WithdrawalAnemic and insert the related record in the database 
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

			if (account == null)
			{
				// publish insufficent balance message to messagebus. Another service can consume this message and notify the user
				// leaving this implementation out as the concept has been proven
				throw new InvalidOperationException("Invalid account");
			}

			await UpdateAccountBalance(account, theEvent.Amount);

			// publish success message to messagebus. Another service can consume this message and notify the user
			// leaving this implementation out, the concept has been proven

			//Not necessary -  for ease of testing
			var withdrawalAnemic = new WithdrawalAnemic
			{
				Id = theEvent.Id,
				AccountId = theEvent.AccountId,
				Amount = theEvent.Amount,
				TransactionDate = theEvent.TransactionDate
			};

			await this.withdrawalRepository.CreateAsync(withdrawalAnemic);
		}

		private async Task UpdateAccountBalance(BankAccountAnemic bankAccount, decimal withdrawalAmount)
		{
			var balance = bankAccount.Balance;
			// if balance is less than amount produce message, 
			if (balance < withdrawalAmount)
			{
				// publish insufficent balance message to messagebus. Another service can consume this message and notify the user
				// leaving this out as the concept has been proven
				throw new InvalidOperationException("Insufficient balance");
			}

			balance -= withdrawalAmount;

			bankAccount.Balance = balance;
			await this.bankAccountRepository.UpdateAsync(bankAccount);
		}
	}
}
