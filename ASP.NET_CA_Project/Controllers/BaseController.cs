using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_CA_Project.Database;
using ASP.NET_CA_Project.Models;
using Microsoft.AspNetCore.Mvc;

// A base controller for common methods and attributes.

namespace ASP.NET_CA_Project.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ShopDBContext db;

        public BaseController(ShopDBContext db)
        {
            this.db = db;
        }

        protected Session GetSession()
        {
            // Get the current session
            string _sessionId = HttpContext.Session.Id;
            Session? session = db.Sessions.FirstOrDefault(sess => sess.Id == _sessionId);
            if (session == null)
            {
                throw new Exception("Session err, check SessionTracker middleware.");
            }
            return session;
        }

        protected User GetSessionUser()
        {
            // Get the user instance of the current user.
            // It can be either loged-in user or a guest.
            // We don't care about it at this stage.
            Session session = GetSession();
            User? sessionUser = db.User.FirstOrDefault(user => user.Id.ToString() == session.UserId);
            if (sessionUser == null)
            {
                throw new Exception("Session err, all sessions must have valid session user.");
            }
            return sessionUser;
        }

        protected List<Order> GetUserOrders()
        {
            // Get the list of unpaid orders of the current user.
            User user = GetSessionUser();
            if (user.Orders != null)
            {
                List<Order> orders = user.Orders.ToList();
                orders.Sort((a, b) => a.Item.ItemName.CompareTo(b.Item.ItemName));
                return orders;
            }
            else
            {
                return new List<Order>();
            }
        }

        protected List<PurchasedOrder> GetUserPurchasedOrders()
        {
            // Get the list of paid orders of the current user.
            User user = GetSessionUser();
            if (user.PurchasedOrders != null)
            {
                List<PurchasedOrder> orders = user.PurchasedOrders.ToList();
                orders.Sort((a, b) => a.Item.ItemName.CompareTo(b.Item.ItemName));
                return orders;
            }
            else
            {
                return new List<PurchasedOrder>();
            }

        }

        protected bool IsSessionUserLoggedIn()
        {
            User user = GetSessionUser();
            return (user.UserName != null);
        }
    }
}

