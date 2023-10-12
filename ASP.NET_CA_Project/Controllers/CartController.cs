using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASP.NET_CA_Project.Database;
using ASP.NET_CA_Project.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP.NET_CA_Project.Controllers
{
    public class CartController : BaseController
    {
        public CartController(ShopDBContext db):base(db)
        { }
        // GET: /<controller>/
        public IActionResult Index(Guid? userID)
        {
            User currentUser = GetSessionUser();
            List<Order> allOrders = (List<Order>)db.Order.ToList();
			//db.Order.Filter
			List<Order> userOrders = allOrders.FindAll(Order => Order.User.Id == currentUser.Id);
			//List<Item> orderItems = (from order in allOrders
			// where order.User.Id == userID
			//select order).ToList();

			/*var userorder = from order in allOrders
                            where order.User.Id == userID
                            select order;*/

			/*var orderItems = from order in userorder
                             group order by order.Item.Id into itemgroup
                             select new
                             {
                                 ItemName = itemgroup.First().Item.ItemName,
                                 Quantity = itemgroup.First().Count,
                                 Price = itemgroup.First().Item.Price * itemgroup.First().Count
                             };*/

			ViewBag.Orders = userOrders;

            return View();
        }

        public IActionResult Checkout()
        {
            if ((string?)ViewData["isLoggedIn"] == "true")
            {
                var obj = new
                {
                    controller = "MyPurchases",
                    action = "Index"
                };
                return RedirectToRoute(obj);
            }
            else
            {
                var obj = new
                {
                    controller = "Login",
                    action = "LoginWithCart"
                    //need more codes and logic
                };
                return RedirectToRoute(obj);
            }
        }


    }
}

