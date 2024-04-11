using Infrastructure.Entities.Market;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ShopManager.DAL.Entities.User;
using ShopManager.Services.Abstract;

namespace ShopManager.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IManagerService _managerService;
        //DI
        public AdminController(IManagerService managerService , UserManager<UserModel> userManager , RoleManager<IdentityRole> roleManager)
        {
            _managerService = managerService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task <IActionResult> GetAllUser() 
        {
            return Json(await _managerService.GetAllUserAsync());
        }

        [HttpPost]
        public async Task <IActionResult> DeleteUser(string managerId)
        {
            return Json(await _managerService.RemoveManagerAsync(managerId));
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string userId )
        {
            return Json(await _managerService.AddRollManagerAsync(userId));
        }

        [HttpGet]
        public async Task <IActionResult> GetOnlyManager()
        {
            return Json(await _managerService.GetManagersAsync());
        }

        [HttpGet]
        public async Task <IActionResult> GetAllMarket()
        {
            return Json(await _managerService.GetAllMarketAsync());
        }

        [HttpPost]
        public async Task <IActionResult> DeleteMarket(int marketId)
        {
            return Json(await _managerService.RemoveMarketAsync(marketId));
        }

        [HttpPost]
        public async Task <IActionResult>AddMarket(string marketName , string marketAddress)
        {
            return Json(await _managerService.AddMarketAsync(marketName, marketAddress));
        }

        [HttpPost]
        public async Task<IActionResult> GetRevenueFilter(string[] managersId, int?[] shopsId, DateOnly fromDate, DateOnly toDate)
        {
            return Json(await _managerService.GetRevenueFilterAsync(managersId, shopsId, fromDate, toDate));
        }

        [HttpGet]
        public async Task<IActionResult> GetRemovedUsers ()
        {
            return Json(await _managerService.GetRemovedUsersAsync());
        }

        [HttpPost]
        public async Task <IActionResult>RemoveUserFinally(string userId)
        {
            return Json(await _managerService.RemoveUsersFinallyAsync(userId));
        }

        [HttpPost]
        public async Task <IActionResult> ReturnUser(string userId)
        {
            return Json(await _managerService.ReturnRemovedUsersAsync(userId));
        }

        [HttpPost]
        public async Task<IActionResult> RemovedMarket()
        {
            return Json(await _managerService.GetRemovedMarketAsync());

        }

        [HttpPost]
        public async Task <IActionResult> RemoveMarketFinnaly(int marketId)
        {
            return Json(await _managerService.RemovedMarketFinallyAsync(marketId));
        }

        [HttpPost]
        public async Task<IActionResult> ReturnMarket(int marketId)
        {
            
            return Json(await _managerService.ReturnRemovedMarketAsync(marketId));
        }
       
    }
}
