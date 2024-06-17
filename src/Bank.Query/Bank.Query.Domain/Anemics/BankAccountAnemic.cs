namespace Bank.Query.Domain.Anemics
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("BankAccount")]
	public class BankAccountAnemic
	{
		[Key]
		public Guid Id { get; set; }
		public decimal Balance { get; set; }
		public DateTimeOffset Modified { get; set; }
	}
}
