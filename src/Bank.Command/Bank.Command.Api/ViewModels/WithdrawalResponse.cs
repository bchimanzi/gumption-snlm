namespace Bank.Command.Api.ViewModels
{
	using Bank.Common.ViewModels;
	using System;

	public class WithdrawalResponse : BaseResponse
	{
		public Guid Id { get; set; }
	}
}
