namespace Bank.Command.Api.Commands
{
	using System.Threading.Tasks;

	using CQRS.Core.Handlers;
	using Bank.Command.Domain.Aggregates;

	/**
 * The CommandHandler class is the concrete colleague class that handles commands by invoking the relevant Aggregate and EventSourcingHandler methods.
 * 
 */
	public class CommandHandler : ICommandHandler
	{
		private readonly IEventSourcingHandler<BankAccountAggregate> eventSourcingHandler;

		public CommandHandler(IEventSourcingHandler<BankAccountAggregate> eventSourcingHandler)
		{
			this.eventSourcingHandler = eventSourcingHandler;
		}

		//TODO: no validatrion needed. an exception is thrown in concrete and the exception will bubble up
		public async Task HandleAsync(WithdrawalCommand command)
		{
			var aggregate = new BankAccountAggregate();

			await this.eventSourcingHandler.SaveAsync(aggregate);
		}
	}
}
