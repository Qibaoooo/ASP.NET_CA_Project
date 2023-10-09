﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_CA_Project.Database;
using ASP.NET_CA_Project.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP.NET_CA_Project.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ShopDBContext db;

        public BaseController(ShopDBContext db)
        {
            this.db = db;
        }

        protected Session? GetSession()
        {
            // Get the current session
            string _sessionId = HttpContext.Session.Id;
            Session? session = db.Sessions.FirstOrDefault(sess => sess.Id == _sessionId);
            return session;
        }

        protected User? GetSessionUser()
        {
            // Get the user instance of the current user.
            Session? session = GetSession();
            User? sessionUser = db.User.FirstOrDefault(user => user.Id == session.UserId);
            return sessionUser;
        }
    }
}
