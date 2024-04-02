using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopManager.DAL.Entities.User;
using ShopManager.Services.Abstract;

namespace ShopManager.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IManagerService _managerService;
        public AdminController(IManagerService managerService , UserManager<UserModel> userManager , RoleManager<IdentityRole> roleManager)
        {
            _managerService = managerService;
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task <IActionResult> GetAllUser() 
        {
            var allUser = await _managerService.GetAllUser();
            return Json(allUser);
        }

        public async Task <IActionResult> DeleteUser(string managerId)
        {

            await _managerService.RemoveManagerAsync(managerId);
            return Json(null);
            
        }
        public async Task<IActionResult> AddRole(string userId  )
        {
            string manager = "Admin";
            await _managerService.AddRollAsync(userId, manager);
            return Json(null);
        }
        public async Task <IActionResult> GetOnlyManager()
        {
            var onlyManager = await _managerService.GetOnlyManager();
            return Json(onlyManager);
        }
        public async Task <IActionResult> GetAllMarket()
        {
            var allMarket = await _managerService.GetAllMarketAsync();
            return Json(allMarket);
        }
        public async Task DeleteMarket(int marketId)
        {
            await _managerService.RemoveMarketAsync(marketId);
        }
        public async Task AddMarket(string marketName , string marketAddress)
        {
           await  _managerService.AddMarketAsync(marketName , marketAddress);
            
        }
    }
}
