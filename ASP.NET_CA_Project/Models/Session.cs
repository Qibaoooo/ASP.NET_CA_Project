using System;
namespace ASP.NET_CA_Project.Models
{
	public class Session
	{
		public Session(Guid sessionId)
		{
			this.Id = sessionId;
			this.User = new User();
		}

		public Guid Id { get; set; }

		public virtual User User { get; set; }
	}
}

