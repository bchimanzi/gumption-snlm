namespace CQRS.Core.Events
{
	using System;

	public abstract class BaseEvent
	{
		protected BaseEvent(string type)
		{
			this.Type = type;
		}
		public int Version { get; set; }
		public string Type { get; set; }
	}

}

