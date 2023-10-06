using ASP.NET_CA_Project.Database;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP.NET_CA_Project.Controllers
{
    public class CartController : Controller
    {

        private ShopDBContext db;

        public CartController(ShopDBContext db)
        {
            this.db = db;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            DBHelper.InjectTestData(db);
            return View();
        }
    }
}

