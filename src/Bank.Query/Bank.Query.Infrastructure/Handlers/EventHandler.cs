namespace Bank.Query.Infrastructure.Handlers
{
	using Bank.Common.Events;
	using System.Threading.Tasks;

	//TODO: Implement
	public class EventHandler : IEventHandler
	{
        public EventHandler()
        {
            
        }
        public Task On(WithdrawalEvent theEvent)
		{
			throw new System.NotImplementedException();
		}
	}
}
