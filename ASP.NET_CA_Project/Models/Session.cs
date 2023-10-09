using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_CA_Project.Models
{
	public class Session
    {
        public Session()
        {
        }

        public Session(string sessionId)
        {
            this.Id = sessionId;
        }

        [Key]
		public string Id { get; set; }

		public virtual Guid UserId { get; set; }
	}
}

