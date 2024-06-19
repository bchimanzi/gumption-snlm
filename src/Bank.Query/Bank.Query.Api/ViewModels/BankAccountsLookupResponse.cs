namespace Bank.Query.Api.ViewModels
{
	using System.Collections.Generic;

	using Bank.Common.ViewModels;
	using Bank.Query.Domain.Anemics;

	public class BankAccountsLookupResponse : BaseResponse
	{
		public List<BankAccountAnemic> BankAccounts { get; set; }
	}
}
