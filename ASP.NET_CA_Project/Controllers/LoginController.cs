using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_CA_Project.Database;
using ASP.NET_CA_Project.Models;
using Microsoft.AspNetCore.Mvc;
using ASP.NET_CA_Project.Database;
using ASP.NET_CA_Project.Models;
using Microsoft.AspNetCore.Identity;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP.NET_CA_Project.Controllers
{
    public class LoginController : BaseController
    {
        public LoginController(ShopDBContext db) : base(db)
        {

        }

        public IActionResult Index()
        {
            ViewData["isLoggedIn"] = (GetSessionUser().UserName != null);
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
            var obj = new
            {
                controller = "Gallery",
                action = "Index"
            };
            return RedirectToRoute(obj);
        }

        public IActionResult Logout()
        {
            Session session = GetSession();

            // create a new guest user for the current session to hold.
            User newGuest = new User();
            session.UserId = newGuest.Id.ToString();

            db.Add(newGuest);
            db.SaveChanges();

            return RedirectToAction(controllerName: "Login", actionName: "Index");
        }

        [HttpPost]
        public IActionResult UserLogin(string userName, string password, string redirectPath)
        {
            if (userName == null)
            {
                return new ContentResult()
                {
                    Content = "user name must not be null",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            if (password == null)
            {
                return new ContentResult()
                {
                    Content = "password must not be null",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            User? user = db.User.FirstOrDefault(u => (u.UserName == userName));
            if (user == null)
            {
                return new ContentResult()
                {
                    Content = "user name not found",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            if (user.Password != password)
            {
                return new ContentResult()
                {
                    Content = $"incorrecnt password for user {user.UserName}",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            // merge carts if necessary;
            CheckAndMergeCart(user);

            // all checks passed, log the user in now.
            UpdateUserSessionInDB(user);

            // remove the guest user
            // RemoveGuestUserFromDB();

            // redirect to previous page
            if (redirectPath != null)
            {
                if (redirectPath.ToLower() == "cart")
                {
                    return RedirectToAction(controllerName: "Cart", actionName: "Index");
                }
            }
            return RedirectToAction(controllerName: "Gallery", actionName: "Index");
        }

        private void UpdateUserSessionInDB(User user)
        {
            Session guestSession = GetSession();
            guestSession.UserId = user.Id.ToString();
            user.latestSession = guestSession;

            db.SaveChanges();
        }

        private void RemoveGuestUserFromDB()
        {
            User sessionUser = GetSessionUser();
            db.Remove(sessionUser);
            db.SaveChanges();
        }

        private void CheckAndMergeCart(User user)
        {
            User guest = GetSessionUser();

            // Now, what we do here is check if the sessionUser
            // has any orders.
            // If it has, we move them to the userInDB's orders.

            if (guest.Orders == null)
            {
                return;
            }

            foreach (Order order in guest.Orders)
            {
                Order? orderWithSameItemName = user.Orders.FirstOrDefault(
                    o => o.Item.ItemName == order.Item.ItemName
                    );
                if (orderWithSameItemName == null)
                {
                    user.Orders.Add(order);
                }
                else
                {
                    orderWithSameItemName.Count =
                        orderWithSameItemName.Count + order.Count;
                }
            }

            db.SaveChanges();
            return;
        }
    }
}

