using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASP.NET_CA_Project.Database;
using ASP.NET_CA_Project.Models;
namespace ASP.NET_CA_Project.Controllers
{
    public class PayController : BaseController
    {
        public PayController(ShopDBContext db) : base(db)
        { }

        public IActionResult Index(Guid? userID)
        {
            if (!IsSessionUserLoggedIn())
            {
                return RedirectToAction(controllerName: "Gallery", actionName: "Index");
            }

            ViewBag.Orders = GetUserOrders();

            return View();
        }

        public IActionResult Pay()
        {
            User user = GetSessionUser();

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
