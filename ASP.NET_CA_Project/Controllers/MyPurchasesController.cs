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
        // GET: /<controller>/
        public IActionResult Index(Guid? userId)
        {
            // Reminder to add in condition if userId is null
            List<PurchasedOrder> AllPurchasedOrders = db.PurchasedOrder.ToList<PurchasedOrder>();
            //var userPurchasedOrders = from order in PurchasedOrders
            //                            where order.User.Id == userId
            //                            select order;

            var purchasedorders = from order in AllPurchasedOrders
                                  group order by order.Item.Id into grouped
                                  select new
                                  {
                                      Item = grouped.First().Item,
                                      DateTime = grouped.First().DateTime,
                                      Quantity = grouped.Count(),
                                      ActivationCodes = grouped.Select(p => p.ActivationCode).ToList()
                                  };

            ViewBag.grouporders = purchasedorders;
            ViewBag.userpurchasedorders = AllPurchasedOrders; 
            
            return View();
            
            // Ridirect to Login Page
            //return Content(""); 
        }
    }
}

