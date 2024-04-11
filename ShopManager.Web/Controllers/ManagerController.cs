using Infrastructure.Entities.Market;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManager.DAL.Entities.User;
using ShopManager.Services.Abstract;

namespace ShopManager.Web.Controllers
{
    [Authorize(Roles ="Manager")]
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

        [HttpGet]
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

        [HttpGet]
        public async Task<IActionResult> GetAllMarket()
        {
            return Json(await _serviceManager.GetAllMarketAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddSales(string ManagerId , int MarketId , decimal Revenue , DateOnly Date)
        {
            return Json(await _serviceManager.AddSalesAsync(ManagerId, MarketId, Revenue, Date));
        }

        [HttpGet]
        public async Task <IActionResult> GetSalesForManager(string managerId)
        {
            return Json(await _serviceManager.GetSalesByManagerAsync(managerId));
        }

        [HttpPost]
       public async Task<IActionResult> ChangeSalesLastMounth(int salesId , decimal newAmount , DateOnly newDate , int newMarketId)
        {
            return Json(await _serviceManager.ChangeSalesLastMounthAsync(salesId, newAmount, newDate, newMarketId));
        }
       
    }
}
