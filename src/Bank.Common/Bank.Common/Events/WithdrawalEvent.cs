namespace Bank.Common.Events
{
	using CQRS.Core.Events;

	public class WithdrawalEvent : BaseEvent
	{
        public WithdrawalEvent() : base(nameof(WithdrawalEvent))
        {
            
        }

        public string Message { get; set; }
    }
}
