namespace Bank.Command.Api.Controllers
{
	using CQRS.Core.Infrastructure;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;

	[ApiController]
	[Route("api/v1/[controller]")]
	public class BankController : ControllerBase
	{
		private readonly ILogger<BankController> logger;
		private readonly ICommandDispatcher commandDispatcher;

		public BankController(ILogger<BankController> logger, ICommandDispatcher commandDispatcher)
		{
			this.logger = logger;
			this.commandDispatcher = commandDispatcher;
		}
	}
}
