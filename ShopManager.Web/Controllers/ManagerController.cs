using Infrastructure.Entities.Market;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopManager.DAL.Entities.User;
using ShopManager.Services.Abstract;

namespace ShopManager.Web.Controllers
{
    public class ManagerController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IManagerService _serviceManager;
        public ManagerController(UserManager<UserModel> userManager , IManagerService serviceManager)
        {
            _userManager = userManager;
            _serviceManager= serviceManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task <IActionResult> GetFullName()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null) 
            {
                var userFullName = user.Name + " " + user.Surname + " " + user.MiddleName;
                var userId = user.Id;
                var result = new { FullName = userFullName, UserId = userId };
                return Json(result);
            }
            return Json(null);
        }
        public async Task<IActionResult> GetAllMarket()
        {
            var allMarket = await _serviceManager.GetAllMarketAsync();
            return Json(allMarket);
        }
        public async Task<IActionResult> AddSales(Sales sales)
        {
            await _serviceManager.AddSalesAsync(sales);
            return Json(null);
        }

    }
}
