using System;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_CA_Project.Models
{
	public class User
	{
        public User()
        {

        }

        public User(string userName, string password)
		{
            Id = Guid.NewGuid();
            UserName = userName;
            Password = password;
		}

        [Required]
        public Guid Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

		// each user can have many orders in 'cart'
		public virtual ICollection<Order> Orders { get; set; }

		// each user can have many purchased orders
        public virtual ICollection<PurchasedOrder> PurchasedOrders { get; set; }
    }
}

