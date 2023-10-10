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
            // List all the Mock Products
            string itemsJsonFile = "Database/MockData/MockProducts.json";
            dynamic itemList = GetJsonData(itemsJsonFile);
            AddMockItems(db, itemList);

            // List all the Mock Users
            string userJsonFile = "Database/MockData/MockUser.json";
            dynamic userList = GetJsonData(userJsonFile);
            AddMockUsers(db, userList);

            // Created Mock Purchased Orders
            AddMockPurchasedOrder(db);

            db.SaveChanges();
        }

        private static void AddMockItems(ShopDBContext db, dynamic itemList)
        { 
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

        private static void AddMockUsers(ShopDBContext db, dynamic itemList)
        {
            foreach (var item in itemList.EnumerateArray())
            {
                string userName = item.GetProperty("userName").GetString();
                string password = item.GetProperty("password").GetString();

                User newUser= new User(userName, password);

                db.Add(newUser);
            }
            db.SaveChanges();
        }

        private static dynamic GetJsonData(string JsonPath) {
            string itemsJson = File.ReadAllText(JsonPath);
            return JsonSerializer.Deserialize<dynamic>(itemsJson);
        }

        private static void AddMockPurchasedOrder(ShopDBContext db) {
            User currentuser = db.User.First();
            
            for (int i= 0;  i < 3; i++) {
                PurchasedOrder purchasedorder = new PurchasedOrder(item: db.Item.Skip(i).First(), user: currentuser);
                db.Add(purchasedorder);
            }
            db.SaveChanges();
        }
    }
}

