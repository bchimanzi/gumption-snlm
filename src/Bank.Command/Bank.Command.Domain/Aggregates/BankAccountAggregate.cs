namespace Bank.Command.Domain.Aggregates
{
	using System;
	using System.Collections.Generic;

	using CQRS.Core.Domain;

	public class BankAccountAggregate : AggregateRoot
	{
		private bool _active;
		private readonly Dictionary<Guid, Tuple<string, string>> _comments = new Dictionary<Guid, Tuple<string, string>>();
		//TODO: Review
		public BankAccountAggregate()
		{
		}

		public bool Active { get => _active; set => _active = value; }
	}
}
