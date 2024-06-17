namespace Bank.Query.Api.Controllers
{
	using CQRS.Core.Infrastructure;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;

	public class BankAccountLookupController : ControllerBase
	{
		private readonly ILogger<BankAccountLookupController> logger;
		private readonly IQueryDispatcher queryDispatcher;

		public BankAccountLookupController(ILogger<BankAccountLookupController> logger, IQueryDispatcher queryDispatcher)
		{
			this.logger = logger;
			this.queryDispatcher = queryDispatcher;
		}
	}
}
