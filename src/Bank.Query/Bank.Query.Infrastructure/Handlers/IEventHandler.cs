namespace Bank.Query.Infrastructure.Handlers
{
	using Bank.Common.Events;
	using System.Threading.Tasks;

	public interface IEventHandler
	{
		Task On(WithdrawalEvent theEvent);
	}
}
