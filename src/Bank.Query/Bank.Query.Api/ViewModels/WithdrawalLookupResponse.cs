namespace Bank.Query.Api.ViewModels
{
	using System.Collections.Generic;

	using Bank.Common.ViewModels;
	using Bank.Query.Domain.Anemics;

	public class WithdrawalLookupResponse : BaseResponse
	{
		public List<WithdrawalAnemic> Withdrawals { get; set; }
	}
}
