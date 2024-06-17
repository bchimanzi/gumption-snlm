namespace Bank.Query.Domain.Anemics
{
	using System;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table(name: "Withdrawal")]
	public class WithdrawalAnemic
	{
		public Guid Id { get; set; }
		public Guid AccountId { get; set; }
		public decimal Amount { get; set; }
		public DateTimeOffset TransactionDate { get; set; }
	}
}
