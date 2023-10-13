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
			List<Order> userOrders = allOrders.FindAll(Order => Order.User.Id == currentUser.Id);

			ViewBag.Orders = userOrders;

            return View();
        }

        public IActionResult RemoveItem(string itemId)
        {
            User? sessionUser = GetSessionUser();
            if (sessionUser == null)
            {
                var err = new
                {
                    Message = "An internal server error occurred.",
                    Details = "Current session has no user in db. Something is wrong."
                };
                return StatusCode(500, err);
            }
            Order? orderToDelete = db.Order.FirstOrDefault(o => o.User.Id == sessionUser.Id && o.Item.Id.ToString() == itemId);
            db.Order.Remove(orderToDelete);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult ChangeItemCount([FromBody]ChangeCountData data)
        {
            string itemId = data.inputId;
            int itemCount = data.userInput;
            User? sessionUser = GetSessionUser();
            /*if (sessionUser == null) 
            {
                var err = new
                {
                    Message = "An internal server error occurred.",
                    Details = "Current session has no user in db. Something is wrong."
                };
                return StatusCode(500, err);
            }*/
            Order? orderToChange = db.Order.FirstOrDefault(o => o.User.Id == sessionUser.Id && o.Item.Id.ToString() == itemId);
            orderToChange.Count = itemCount;
            db.SaveChanges();
            return RedirectToAction("Index");
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

