namespace Bank.Query.Api.Controllers
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Http;
	using Microsoft.Extensions.Logging;

	using Bank.Query.Api.Queries;
	using Bank.Common.ViewModels;
	using CQRS.Core.Infrastructure;
	using Bank.Query.Api.ViewModels;
	using Bank.Query.Domain.Anemics;

	[ApiController]
	[Route("api/v1/[controller]")]
	public class WithdrawalLookupController : ControllerBase
	{
		private readonly ILogger<WithdrawalLookupController> logger;
		private readonly IQueryDispatcher<WithdrawalAnemic> queryDispatcher;

		public WithdrawalLookupController(ILogger<WithdrawalLookupController> logger, IQueryDispatcher<WithdrawalAnemic> queryDispatcher)
		{
			this.logger = logger;
			this.queryDispatcher = queryDispatcher;
		}

		[HttpGet("byId/{withdrawalId}")]
		public async Task<ActionResult> GetByIdAsync(Guid withdrawalId)
		{
			try
			{
				var withdrawals = await this.queryDispatcher.SendAsync(new FindWithdrawalByIdQuery { Id = withdrawalId });

				if (withdrawals == null || !withdrawals.Any())
					return NoContent();

				return Ok(new WithdrawalLookupResponse
				{
					Withdrawals = withdrawals,
					Message = "Successfully returned withdrawals!"
				});
			}
			catch (Exception ex)
			{
				const string ERROR_MESSAGE = "Error while processing request to find withdrawal by ID!";
				return ErrorResponse(ex, ERROR_MESSAGE);
			}
		}

		[HttpGet("byAccountId/{accountId}")]
		public async Task<ActionResult> GetWithdrawalsByAccountIdAsync(Guid accountId)
		{
			try
			{
				var withdrawals = await this.queryDispatcher.SendAsync(new FindWithdrawalsByAccountIdQuery { AccountId = accountId });

				if (withdrawals == null || !withdrawals.Any())
					return NoContent();

				return Ok(new WithdrawalLookupResponse
				{
					Withdrawals = withdrawals,
					Message = "Successfully returned withdrawals!"
				});
			}
			catch (Exception ex)
			{
				const string ERROR_MESSAGE = "Error while processing request to find withdrawal by accountId!";
				return ErrorResponse(ex, ERROR_MESSAGE);
			}
		}

		private ActionResult ErrorResponse(Exception ex, string errorMessage)
		{
			this.logger.LogError(ex, errorMessage);

			return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
			{
				Message = errorMessage
			});
		}
	}
}
