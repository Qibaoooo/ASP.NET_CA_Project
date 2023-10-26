using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_CA_Project.Database;
using ASP.NET_CA_Project.Models;
using Microsoft.AspNetCore.Mvc;

// This controller is purely for debugging purpose.
// not supposed to be serving this route in production.
// only use this when developing.

namespace ASP.NET_CA_Project.Controllers
{
    public class DebugController : BaseController
    {
        public DebugController(ShopDBContext db): base(db)
        {

        }
        
        public IActionResult Index()
        {
            ViewBag.currentSession = this.GetSession();
            ViewBag.currentUser = this.GetSessionUser();
            return View();
        }

        public IActionResult DebugCheckout()
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

            return RedirectToAction("Index");
        }
    }
}

