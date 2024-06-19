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
		private readonly IEventSourcingHandler<WithdrawalAggregate> eventSourcingHandler;

		public CommandHandler(IEventSourcingHandler<WithdrawalAggregate> eventSourcingHandler)
		{
			this.eventSourcingHandler = eventSourcingHandler;
		}

		public async Task HandleAsync(WithdrawalCommand command)
		{
			var aggregate = new WithdrawalAggregate(id: command.Id, accountId: command.AccountId, amount: command.Amount);

			await this.eventSourcingHandler.SaveAsync(aggregate);
		}

		public async Task HandleAsync(RestoreReadDatabaseCommand command)
		{
			await this.eventSourcingHandler.RepublishEventsAsync();
		}
	}
}
