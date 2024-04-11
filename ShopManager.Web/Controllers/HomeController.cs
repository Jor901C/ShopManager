using Microsoft.AspNetCore.Mvc;

namespace ShopManager.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
