namespace Bank.Query.Api.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;

	using CQRS.Core.Infrastructure;
	using Bank.Query.Domain.Anemics;

	[ApiController]
	[Route("api/v1/[controller]")]
	public class BankAccountLookupController : ControllerBase
	{
		private readonly ILogger<BankAccountLookupController> logger;
		private readonly IQueryDispatcher<BankAccountAnemic> queryDispatcher;

		public BankAccountLookupController(ILogger<BankAccountLookupController> logger, IQueryDispatcher<BankAccountAnemic> queryDispatcher)
		{
			this.logger = logger;
			this.queryDispatcher = queryDispatcher;
		}
	}
}
