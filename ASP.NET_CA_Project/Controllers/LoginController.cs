using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_CA_Project.Database;
using ASP.NET_CA_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP.NET_CA_Project.Controllers
{
    public class LoginController : BaseController
    {
        public LoginController(ShopDBContext db) : base(db)
        {

        }

        public IActionResult Index(string redirect_url)
        {
            if (IsSessionUserLoggedIn())
            {
                return RedirectToAction(controllerName: "Gallery", actionName: "Index");
            }

            List<Order> orders = GetUserOrders();
            ViewData["ordersInCart"] = orders;

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
        public IActionResult UserLogin(string userName, string password, bool mergeCart)
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

            if (mergeCart == true)
            {
                // merge carts if necessary;
                CheckAndMergeCart(user);
            }

            // all checks passed, log the user in now.
            UpdateUserSessionInDB(user);

            // redirect to previous page:
            // if the current session does not have a 'redirectController' recorded,
            // we redirect the user to Gallery.
            // if the current session have a 'redirectController' recorded,
            // use it as the redirect destination.
            string? redirectController = HttpContext.Session.GetString("redirectController");
            return Json(new { redirectController = (redirectController == null) ? "Gallery" : redirectController });
        }

        private void UpdateUserSessionInDB(User user)
        {
            Session guestSession = GetSession();

            guestSession.UserId = user.Id.ToString();
            user.latestSession = guestSession;

            db.SaveChanges();
        }

        private void CheckAndMergeCart(User user)
        {
            User guest = GetSessionUser();

            // Now, what we do here is check if the guest
            // has any orders.
            // If it has, we move them to the logged-in user's orders.

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

