using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopManager.DAL.Entities.User;
using ShopManager.Services.Abstract;

namespace ShopManager.Web.Controllers
{
    public class ManagersController : Controller
    {
        private readonly IManagerService _managerService;
        public ManagersController(IManagerService managerService)
        {
            _managerService = managerService;
        }
        public IActionResult Exeptation()
        {
            return View();
        }

       
    }
}
