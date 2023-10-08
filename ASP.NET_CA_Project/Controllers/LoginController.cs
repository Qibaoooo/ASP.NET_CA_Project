using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP.NET_CA_Project.Controllers
{
    public class LoginController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index(bool isLoggedIn)
        {
            ViewData["isLoggedIn"] = isLoggedIn;
            return View();
        }

        public IActionResult GuestContinue()
        {
            ViewData["isLoggedIn"] = false;
            var obj = new
            {
                controller = "Gallery",
                action = "Index"
            };
            return RedirectToRoute(obj);
        }

        public IActionResult Logout()
        {
            ViewData["isLoggedIn"] = false;
            return RedirectToAction("Index");
        }

        public IActionResult LoginWithCart()
        {
            //need more codes and logic
            return RedirectToAction("Index");
        }
    }
}

