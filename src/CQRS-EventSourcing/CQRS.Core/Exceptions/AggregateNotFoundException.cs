namespace CQRS.Core.Exceptions
{
	using System;

	public class AggregateNotFoundException : Exception
	{
		public AggregateNotFoundException(string message) : base(message)
		{
		}
	}
}
