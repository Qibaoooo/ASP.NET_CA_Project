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
    public class MyPurchasesController : BaseController
    {
        
        public MyPurchasesController(ShopDBContext db): base(db)
        {
        }
        
        public IActionResult Index(Guid? userId)
        {
            if (!IsSessionUserLoggedIn())
            {
                return RedirectToAction(controllerName: "Gallery", actionName: "Index");
            }

            List<PurchasedOrder> allPurchasedOrders = GetUserPurchasedOrders();

            var groupedOrders = from order in allPurchasedOrders
                                group order by order.Item.Id into grouped
                                  select new
                                  {
                                      Item = grouped.First().Item,
                                      DateTime = grouped.First().DateTime,
                                      Quantity = grouped.Count(),
                                      ActivationCodes = grouped.Select(p => p.ActivationCode).ToList()
                                  };

            ViewBag.allPurchasedOrders = allPurchasedOrders;
            ViewBag.groupedOrders = groupedOrders;

            return View();
        }
    }
}

