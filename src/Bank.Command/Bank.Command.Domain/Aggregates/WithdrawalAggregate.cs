namespace Bank.Command.Domain.Aggregates
{
	using System;

	using CQRS.Core.Domain;
	using Bank.Common.Events;


	public class WithdrawalAggregate : AggregateRoot
	{
	
		public WithdrawalAggregate()
		{
		}

		//constructor always handles the command the creates new instance of the aggregate
		public WithdrawalAggregate(Guid id, Guid accountId, decimal amount)
		{
			if (amount <= 0m)
			{
				throw new InvalidOperationException($"The value of {nameof(amount)} is invalid. Please provide a valid {nameof(amount)}");
			}

			RaiseEvent(new WithdrawalEvent
			{
				Id = id,
				AccountId = accountId,
				Amount = amount,
				TransactionDate = DateTimeOffset.UtcNow
			});
		}

		public void Apply(WithdrawalEvent theEvent)
		{
			_id = theEvent.AccountId;
		}

	}
}
