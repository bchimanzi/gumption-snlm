namespace Bank.Command.Tests
{
	using System;
	using System.Threading.Tasks;

	using Bank.Command.Api.Commands;

	[TestClass]
	public class WithdrawCommandTests : TestBase
	{
		[TestMethod]
		public async Task Can_Send_WithdrawalRequest()
		{
			//Arrange
			var withdrawalCommand = new WithdrawalCommand
			{
				Id = Guid.NewGuid(),
				AccountId = Guid.NewGuid(),
				Amount = 10m,
				TransactionDate = DateTimeOffset.UtcNow

			};

			//Act
			await this.commandDispatcher.SendAsync(withdrawalCommand);

			//Assert
			Assert.IsTrue(true);
		}

		[TestMethod]
		public async Task Can_Fail_InvalidAmount()
		{
			//Arrange
			var withdrawalCommand = new WithdrawalCommand
			{
				Id = Guid.NewGuid(),
				AccountId = Guid.NewGuid(),
				Amount = 0m,
				TransactionDate = DateTimeOffset.UtcNow

			};
			//Act
			Func<Task> act = () => this.commandDispatcher.SendAsync(withdrawalCommand);

			//Assert
			await Assert.ThrowsExceptionAsync<InvalidOperationException>(act);
		}
	}
}