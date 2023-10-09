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
    public class GalleryController : Controller
    {
        private readonly ShopDBContext db;

        public GalleryController(ShopDBContext db)
        {
            this.db = db;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Item> allItems = (List<Item>)db.Item.ToList();
            ViewBag.items = allItems;

            return View();
        }

        [HttpPost]
        public IActionResult AddToCart(string itemId)
        {
            // An AddToCart must have itemId sent as data
            if (itemId == null)
            {
                return Json(new { });
            }

            // Check if the itemId is valid
            Item? item = db.Item.FirstOrDefault(i => i.Id.ToString() == itemId);
            if (item == null)
            {
                var err = new
                {
                    Message = "An internal server error occurred.",
                    Details = $"itemId {itemId} not found. Check if item exists in db."
                };
                return StatusCode(500, err);
            }

            // Get the current session; we don't concern about user/guest for now
            string _sessionId = HttpContext.Session.Id;
            Session? session = db.Sessions.FirstOrDefault(sess => sess.Id == _sessionId);
            if (session == null)
            {
                var err = new
                {
                    Message = "An internal server error occurred.",
                    Details = "Session is not found in db. Check middleware logic and session db."
                };
                return StatusCode(500, err);
            }

            // Add new order to db. Does not matter if user is logged in or not.
            User? sessionUser = db.User.FirstOrDefault(user => user.Id == session.UserId);
            if (sessionUser == null)
            {
                var err = new
                {
                    Message = "An internal server error occurred.",
                    Details = "Current session has no user in db. Something is wrong."
                };
                return StatusCode(500, err);
            }

            Order newOrder = new Order(item, sessionUser, 1);

            db.Order.Add(newOrder);
            db.SaveChanges();

            return Json(new { newOrderId = newOrder.Id });
        }

    }
}

