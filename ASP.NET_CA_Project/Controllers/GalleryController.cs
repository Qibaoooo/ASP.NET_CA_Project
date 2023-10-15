using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASP.NET_CA_Project.Database;
using ASP.NET_CA_Project.Models;


namespace ASP.NET_CA_Project.Controllers
{
    public class GalleryController : BaseController
    {
        public GalleryController(ShopDBContext db) : base(db)
        {
        }

        public IActionResult Index()
        {
            List<Item> allItems = (List<Item>)db.Item.ToList();
            allItems.Sort((a, b) => a.ItemName.CompareTo(b.ItemName));
            ViewBag.items = allItems;

            List <Order> userOrders = GetUserOrders();
            ViewBag.itemCountInCart = 0;
            foreach (Order order in userOrders)
            {
                ViewBag.itemCountInCart = ViewBag.itemCountInCart + order.Count;
            }

            ViewBag.isUserLoggedIn = IsSessionUserLoggedIn();
            ViewBag.user = GetSessionUser();

            return View();
        }

        [HttpPost]
        public IActionResult AddToCart(string itemId)
        {
            // An AddToCart must have itemId sent as data
            if (itemId == null)
            {
                return Json(new { info = "must supply itemId." });
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
            User? sessionUser = GetSessionUser();
            if (sessionUser == null)
            {
                var err = new
                {
                    Message = "An internal server error occurred.",
                    Details = "Current session has no user in db. Something is wrong."
                };
                return StatusCode(500, err);
            }

            // check if user has added this item before, if so, increment the count
            Order? existingOrder = db.Order.FirstOrDefault(o => o.User.Id == sessionUser.Id && o.Item.Id.ToString() == itemId);
            if (existingOrder != null)
            {
                if (existingOrder.Count < 999)
                {
                    existingOrder.Count++;
                }
                else
                {
                    var err = new
                    {
                        Message = "The quantity of the item in cart reaches the limit.",
                        Details = "There are already 999 this items in cart."
                    };
                    return StatusCode(500, err);
                }
            }
            else
            {
                Order newOrder = new Order(item, sessionUser, 1);
                db.Order.Add(newOrder);
            }

            db.SaveChanges();

            return Json(new { info = "item added" });
        }
    }
}

