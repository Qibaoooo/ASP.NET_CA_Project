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
            UserId = "";
        }

        [Key]
		public string Id { get; set; }

		public string UserId { get; set; }
	}
}

