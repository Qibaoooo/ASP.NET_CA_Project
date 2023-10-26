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

            // check if the session is already being tracked in db
            Session? sessionInDB = db.Sessions.FirstOrDefault(
                sess => sess.Id == _sessionId
                );

            if (sessionInDB == null)
            {
                // the session is not tracked yet, add it to db
                Session newSession = new Session(_sessionId);
                // create guest user for the session
                User guest = new User();
                newSession.UserId = guest.Id.ToString();

                Console.WriteLine($"guest user created: {guest.Id} for session {newSession.Id}");

                // add & save
                db.Add(guest);
                db.Add(newSession);
                db.SaveChanges();
            }

            TrackRedirectUrl(httpContext, db);

            httpContext.Session.SetString("tracked", "Yes");

            return _next(httpContext);
        }

        private void TrackRedirectUrl(HttpContext httpContext, ShopDBContext db)
        {
            string? controllerName = (string?)httpContext.GetRouteValue("controller");

            // used later in Login controller
            // if user is visiting Login page, do NOT update.
            if (controllerName != "Login" && controllerName != null)
            {
                httpContext.Session.SetString("redirectController", controllerName);
            }

            return;
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

