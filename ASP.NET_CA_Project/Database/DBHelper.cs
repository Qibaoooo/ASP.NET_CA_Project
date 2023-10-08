using System;
using ASP.NET_CA_Project.Models;
namespace ASP.NET_CA_Project.Database
{
    public class DBHelper
    {
        public DBHelper()
        {
        }

        public static void InjectTestData(ShopDBContext db)
        {
            Item item = new Item(itemName: ".NET Charts", price: 99, description: "Brings powerful ...");

            User user = new User(userName: "Murakami", password: "123123");
            User userWithOrders = new User(userName: "Haruki", password: "123123");
            User userWithPurchasedOrders = new User(userName: "Kafka", password: "123123");

            userWithOrders.Orders = new List<Order> { new
                Order(item: item, user: userWithOrders, count:5)};

            userWithPurchasedOrders.PurchasedOrders = new List<PurchasedOrder> { new
                PurchasedOrder(item: item, user: userWithPurchasedOrders)};



            db.Add(item);
            db.Add(user);
            db.Add(userWithOrders);
            db.Add(userWithPurchasedOrders);

            db.SaveChanges();
        }
    }
}

