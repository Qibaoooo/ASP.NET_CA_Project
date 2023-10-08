using System;
using System.Collections.Generic;
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
            List<Item> items = GetItems();
            Item item = items[0];

            User user = new User(userName: "Murakami", password: "123123");
            User userWithOrders = new User(userName: "Haruki", password: "123123");
            User userWithPurchasedOrders = new User(userName: "Kafka", password: "123123");

            userWithOrders.Orders = new List<Order> { new
                Order(item: item, user: userWithOrders, count:5)};

            userWithPurchasedOrders.PurchasedOrders = new List<PurchasedOrder> { new
                PurchasedOrder(item: item, user: userWithPurchasedOrders)};



            foreach (var i in items)
            {
                db.Add(i);
            }
            db.Add(user);
            db.Add(userWithOrders);
            db.Add(userWithPurchasedOrders);

            db.SaveChanges();
        }

        private static List<Item> GetItems()
        {
            List<Item> items = new List<Item> {
                new Item(itemName: ".NET Charts", price: 99, description: "Brings powerful charting capabilities to your .NET applications"),
                new Item(itemName: ".NET PayPal", price: 69, description: "Integrate your .NET apps with PayPal the easy way!"),
                new Item(itemName: ".NET ML", price: 299, description: "Supercharged .NET machine learning libraries."),
                new Item(itemName: ".NET Analytics", price: 299, description: "Performs data mining and analytics easily in .NET"),
                new Item(itemName: ".NET Logger", price: 49, description: "Logs and aggregates events easily in your .NET apps"),
                new Item(itemName: ".NET Numerics", price: 199, description: "Powerful numerical methods for your .NET simulations")
            };

            return items;
        }
    }
}

