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
        
        public IActionResult Index(Guid? userID)
        {
			ViewBag.Orders = GetUserOrders();

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
        //public IActionResult ChangeItemCount([FromBody]ChangeCountData data)
        public IActionResult ChangeItemCount(string inputId, int userInput)
        {
            string itemId = inputId;
            int itemCount = userInput;
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
            if (!IsSessionUserLoggedIn())
            {
                // On JS side, alert user "you must login"
                return RedirectToAction(controllerName: "Login", actionName: "Index");
            }

            User user = GetSessionUser();

            if (user.Orders.Count() < 1)
            {
                // On JS side, alert user "you must add items to cart first"
                return RedirectToAction(controllerName: "Gallery", actionName: "Index");
            }

            foreach (Order order in user.Orders)
            {
                for (int i = 0; i < order.Count; i++)
                {
                    PurchasedOrder newPaidOrder = new PurchasedOrder(order.Item, user);
                    db.Add(newPaidOrder);
                }

                db.Remove(order);
            }

            db.SaveChanges();

            return RedirectToAction(controllerName: "MyPurchases", actionName: "Index");
        }


    }
}

