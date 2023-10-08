using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_CA_Project.Models
{
    public class PurchasedOrder
    {
        public PurchasedOrder()
        {

        }

        public PurchasedOrder(Item item, User user)
        {
            Id = Guid.NewGuid();
            Item = item;
            User = user;

            // here generates the ActivationCode for new purchased order
            ActivationCode = Guid.NewGuid();
            
            DateTime = DateTime.Now;
        }

        [Required]
        public Guid Id { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public Guid ActivationCode { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

    }
}

