﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_CA_Project.Models
{
    public class User
    {
        public User()
        {
            // Guest will use this constructor
            Id = Guid.NewGuid();
        }

        public User(string userName, string password)
        {
            Id = Guid.NewGuid();
            UserName = userName;
            Password = password;
        }

        [Required]
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        // each user can have many orders in 'cart'
        public virtual ICollection<Order> Orders { get; set; }

        // each user can have many purchased orders
        public virtual ICollection<PurchasedOrder> PurchasedOrders { get; set; }

        public virtual Session latestSession { get; set; }
    }
}

