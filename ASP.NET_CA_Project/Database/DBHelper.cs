using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using ASP.NET_CA_Project.Models;
using System.Text.Json;

namespace ASP.NET_CA_Project.Database
{
    public class DBHelper
    {
        public DBHelper()
        {
        }

        public static void InjectTestData(ShopDBContext db)
        {

            AddMockItems(db);

            AddMockUsers(db);

            // add users with orders
            User userWithOrders = new User(userName: "Murakami Haruki", password: "123123");
            User userWithPurchasedOrders = new User(userName: "Kafka", password: "123123");

            userWithOrders.Orders = new List<Order> { new
                Order(item: db.Item.First(), user: userWithOrders, count:5)};

            userWithPurchasedOrders.PurchasedOrders = new List<PurchasedOrder> { new
                PurchasedOrder(item: db.Item.First(), user: userWithPurchasedOrders)};

            db.Add(userWithOrders);
            db.Add(userWithPurchasedOrders);

            db.SaveChanges();
        }

        private static void AddMockItems(ShopDBContext db)
        {
            string itemsJsonFile = "Database/MockData/MockProducts.json";

            string itemsJson = File.ReadAllText(itemsJsonFile);

            dynamic itemList = JsonSerializer.Deserialize<dynamic>(itemsJson);

            foreach (var item in itemList.EnumerateArray())
            {
                string itemName = item.GetProperty("productName").GetString();
                string description = item.GetProperty("description").GetString();
                float price = (float)item.GetProperty("price").GetDouble();

                Item newItem = new Item(itemName, price, description);

                // replace with this once we get proper image names
                //newItem.Image = item.GetProperty("imageUrl").GetString();
                newItem.Image = "temp.jpg";

                db.Add(newItem);
            }

            db.SaveChanges();
        }

        private static void AddMockUsers(ShopDBContext db)
        {
            string itemsJsonFile = "Database/MockData/MockUser.json";

            string itemsJson = File.ReadAllText(itemsJsonFile);

            dynamic itemList = JsonSerializer.Deserialize<dynamic>(itemsJson);

            foreach (var item in itemList.EnumerateArray())
            {
                string userName = item.GetProperty("userName").GetString();
                string password = item.GetProperty("password").GetString();

                User newUser= new User(userName, password);

                db.Add(newUser);
            }

            db.SaveChanges();

        }

    }
}

