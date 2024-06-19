namespace Bank.Command.Api.Controllers
{
	using Bank.Common.ViewModels;
	using CQRS.Core.Infrastructure;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;
	using System.Threading.Tasks;
	using System;
	using Bank.Command.Api.Commands;

	[ApiController]
	[Route("api/v1/[controller]")]
	public class RestoreReadDatabaseController : ControllerBase
	{
		private readonly ILogger<RestoreReadDatabaseController> _logger;
		private readonly ICommandDispatcher _commandDispatcher;

		public RestoreReadDatabaseController(ILogger<RestoreReadDatabaseController> logger, ICommandDispatcher commandDispatcher)
		{
			_logger = logger;
			_commandDispatcher = commandDispatcher;
		}

		[HttpPost]
		public async Task<ActionResult> RestoreReadDbAsync()
		{
			try
			{
				await _commandDispatcher.SendAsync(new RestoreReadDatabaseCommand());

				return StatusCode(StatusCodes.Status201Created, new BaseResponse
				{
					Message = "Read database restore request completed successfully!"
				});
			}
			catch (InvalidOperationException ex)
			{
				_logger.Log(LogLevel.Warning, ex, "Client made a bad request!");
				return BadRequest(new BaseResponse
				{
					Message = ex.Message
				});
			}
			catch (Exception ex)
			{
				const string SAFE_ERROR_MESSAGE = "Error while processing request to restore read database!";
				_logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

				return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
				{
					Message = SAFE_ERROR_MESSAGE
				});
			}
		}
	}
}
