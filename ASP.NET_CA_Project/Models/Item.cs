using System;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_CA_Project.Models
{
    public class Item
    {
        public Item()
        {
            // need to keep empty constructor here for EF init to work
        }

        public Item(string itemName, float price, string description)
        {
            Id = Guid.NewGuid();
            ItemName = itemName;
            Price = price;
            Description = description;
        }

        [Required]
        public Guid Id { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public string Description { get; set; }

        public string? Image { get; set; }

    }

    public class ItemList
    {
        public List<Item> Items { get; set; }
    }
}

