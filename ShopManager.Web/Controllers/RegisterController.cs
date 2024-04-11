using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopManager.DAL.Entities.User;
using ShopManager.Services.Abstract;

namespace ShopManager.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        public RegisterController(UserManager<UserModel> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Exeptation()
        {
            return View();
        }

        public async Task<IActionResult> WhatRole()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (roles.Contains("Manager"))
                {
                    return RedirectToAction("Index", "Manager");
                }
            }
                return RedirectToAction("Exeptation", "Register");
            
        }



    }
}
