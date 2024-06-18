namespace Bank.Command.Api.Controllers
{

	using System;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Http;
	using Microsoft.Extensions.Logging;

	using Bank.Common.ViewModels;
	using CQRS.Core.Infrastructure;
	using Bank.Command.Api.Commands;
	using Bank.Command.Api.ViewModels;

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

		[HttpPost("withdraw")]
		public async Task<ActionResult> WithdrawAsync(WithdrawalCommand command)
		{
			var id = Guid.NewGuid();
			try
			{
				command.Id =id;

				//dispatch command to the handler method
				await this.commandDispatcher.SendAsync(command);

				return StatusCode(StatusCodes.Status201Created, new WithdrawalResponse
				{
					Id = id,
					Message = "Withdrawal request completed successfully!"
				});
			}
			catch (InvalidOperationException ex)
			{
				this.logger.Log(LogLevel.Warning, ex, "Invalid Request!");
				return BadRequest(new BaseResponse
				{
					Message = ex.Message
				});
			}
			catch (Exception ex)
			{
				const string ERROR_MESSAGE = "Error while processing withdrawal request";
				this.logger.Log(LogLevel.Error, ex, ERROR_MESSAGE);

				return StatusCode(StatusCodes.Status500InternalServerError, new WithdrawalResponse
				{
					Id = id,
					Message = ERROR_MESSAGE
				});
			}
		}
	}
}
