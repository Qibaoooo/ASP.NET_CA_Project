using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_CA_Project.Database;
using ASP.NET_CA_Project.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ASP.NET_CA_Project.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SessionTracker
    {
        private readonly RequestDelegate _next;

        public SessionTracker(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, ShopDBContext db)
        {
            // get session id of current request
            string _sessionId = httpContext.Session.Id;

            // check if the session is already being tracked
            if (!(db.Sessions.Any(sess => sess.Id == _sessionId)))
            {
                // if not tracked, add it to db
                Session newSession = new Session(_sessionId);
                // create guest user for the session
                User guest = new User();
                newSession.UserId = guest.Id;

                // and & save
                db.Add(guest);
                db.Add(newSession);
                db.SaveChanges();

            }

            httpContext.Session.SetString("tracked", "Yes");

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SessionTrackerExtensions
    {
        public static IApplicationBuilder UseMiddlewareClassTemplate(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionTracker>();
        }
    }
}

