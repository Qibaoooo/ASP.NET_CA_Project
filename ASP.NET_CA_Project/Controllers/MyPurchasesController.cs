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
    public class MyPurchasesController : Controller
    {
        private readonly ShopDBContext db;
        public MyPurchasesController(ShopDBContext db)
        {
            this.db = db;
        }
        // GET: /<controller>/
        public IActionResult Index(string userId)
        {
            List<PurchasedOrder> PurchasedOrders = db.PurchasedOrder.ToList<PurchasedOrder>();
            
            return View();
        }
    }
}

