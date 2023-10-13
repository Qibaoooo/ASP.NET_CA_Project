using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASP.NET_CA_Project.Database;
using ASP.NET_CA_Project.Models;
using Microsoft.AspNetCore.Identity;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP.NET_CA_Project.Controllers
{
    public class LoginController : BaseController
    {
        public LoginController(ShopDBContext db) : base(db) { }
        // GET: /<controller>/
        public IActionResult Index(bool isLoggedIn)
        {
            ViewData["isLoggedIn"] = isLoggedIn;
            return View();
        }

        public IActionResult AuthenticateUser(string username, string password) {
            //Check if user is in database
            User? user = db.User.FirstOrDefault(x => x.UserName == username);
            //Check if session has an ongoing cart and user has an existing cart

            if (user == null)
            {
                return Json(new { }); //User not valid, please try again rendered into login html
            }
            else if (user != null && user.Password == password)
            { 
                return Json(new { }); //Return success and redirect
            }
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

