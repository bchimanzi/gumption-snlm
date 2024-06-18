namespace Bank.Command.Api.Commands
{
	using System;

	using CQRS.Core.Commands;

	public class WithdrawalCommand : BaseCommand
	{
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset TransactionDate	{ get; set; }
    }
}
