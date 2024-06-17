namespace Bank.Command.Infrastructure.Dispatchers
{
	using System;
	using System.Threading.Tasks;
	using System.Collections.Generic;

	using CQRS.Core.Commands;
	using CQRS.Core.Infrastructure;

	public class CommandDispatcher : ICommandDispatcher
	{
		//dictionary to hold all command handler methods as function delegates
		private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = new Dictionary<Type, Func<BaseCommand, Task>>();
		public CommandDispatcher()
		{

		}

		public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand
		{
			if (_handlers.ContainsKey(typeof(T)))
			{
				throw new IndexOutOfRangeException("Handler already registered for " + typeof(T).Name);
			}
			_handlers.Add(typeof(T), command => handler((T)command));
		}

		public async Task SendAsync(BaseCommand command)
		{
			if (_handlers.TryGetValue(command.GetType(), out Func<BaseCommand, Task> handler))
			{
				await handler(command);
			}
			else
			{
				throw new ArgumentNullException(nameof(handler), "No command handler was registered");
			}
		}
	}
}
