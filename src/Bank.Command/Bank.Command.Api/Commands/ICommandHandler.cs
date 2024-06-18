namespace Bank.Command.Api.Commands
{
	using System.Threading.Tasks;

	public interface ICommandHandler
	{
		Task HandleAsync(WithdrawalCommand command);
	}
}
