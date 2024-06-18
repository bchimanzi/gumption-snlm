namespace Bank.Common.Events
{
	using System;

	using CQRS.Core.Events;

	public class WithdrawalEvent : BaseEvent
	{
		public WithdrawalEvent() : base(nameof(WithdrawalEvent))
		{

		}
		public Guid Id { get; set; }
		public Guid AccountId { get; set; }
		public decimal Amount { get; set; }
		public DateTimeOffset TransactionDate { get; set; }
	}
}
