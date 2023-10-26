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
        public CartController(ShopDBContext db) : base(db)
        { }

        public IActionResult Index(Guid? userID)
        {
            ViewBag.Orders = GetUserOrders();

            return View();
        }

        [HttpPost]
        public IActionResult RemoveOrder(string itemId)
        {
            // remove the order which has the item
            // that matches the supplied itemId
            List<Order> orders = GetUserOrders();
            Order? orderToRemove = orders.FirstOrDefault(
                o => o.Item.Id.ToString() == itemId
                );
            if (orderToRemove == null)
            {
                return Json(new { success = 0, err = "item or found in user orders." });
            }

            db.Order.Remove(orderToRemove);
            db.SaveChanges();

            return Json(new { success = 1, orderRemoved = orderToRemove.Id });
        }

        public IActionResult RemoveAllOrder()
        {
            List<Order> orders = GetUserOrders();
            foreach(Order order in orders) 
            {
                db.Order.Remove(order);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeItemCount(string itemId, int newCount)
        {
            Order? orderToChange = GetUserOrders().FirstOrDefault(o => o.Item.Id.ToString() == itemId);
            if (orderToChange == null)
            {
                return Json(new { success = 0, err = "item not found in user orders." });
            }

            if (newCount == 0)
            {
                db.Order.Remove(orderToChange);
                db.SaveChanges();

                return Json(new { success = 1, orderRemoved = orderToChange.Id });
            }

            orderToChange.Count = newCount;
            db.SaveChanges();
            return Json(new { success = 1, orderChanged = orderToChange.Id, newOrderCount = newCount });
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

            return RedirectToAction(controllerName: "Pay", actionName: "Index");
        }

    }
}

