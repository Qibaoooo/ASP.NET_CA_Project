using ASP.NET_CA_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_CA_Project.Database
{
    public class ShopDBContext: DbContext
    {
        public ShopDBContext(DbContextOptions<ShopDBContext> options): base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<PurchasedOrder> PurchasedOrder { get; set; }
        public DbSet<Session> Sessions { get; set; }
    }
}

