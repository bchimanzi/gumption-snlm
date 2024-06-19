namespace CQRS.Core.Infrastructure
{
	using System;
	using System.Threading.Tasks;

	using CQRS.Core.Commands;

	public interface ICommandDispatcher
	{
	  void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand;
		Task SendAsync(BaseCommand command);
	}
}
