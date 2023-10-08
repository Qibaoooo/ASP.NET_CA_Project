using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_CA_Project.Models
{
    public class Order
    {
        public Order()
        {

        }

        public Order(Item item, User user, int count)
        {
            Id = Guid.NewGuid();
            Item = item;
            User = user;
            Count = count;
        }

        [Required]
        public Guid Id { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public int Count { get; set; }

    }
}

