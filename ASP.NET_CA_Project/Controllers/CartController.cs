using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP.NET_CA_Project.Controllers
{
    public class CartController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            if ((string?)ViewData["isLoggedIn"] == "true")
            {
                var obj = new
                {
                    controller = "MyPurchases",
                    action = "Index"
                };
                return RedirectToRoute(obj);
            }
            else
            {
                var obj = new
                {
                    controller = "Login",
                    action = "LoginWithCart"
                    //need more codes and logic
                };
                return RedirectToRoute(obj);
            }
        }
    }
}

