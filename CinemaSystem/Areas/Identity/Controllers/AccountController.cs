using CinemaSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinemaSystem.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);
           var result=  await  _userManager.CreateAsync(new()
              {
                Name = registerVM.Name,
                Email=registerVM.Email,
                Address=registerVM.Address,
              UserName = registerVM.UserName,
               },registerVM.Password);
          
            if (!result.Succeeded)
            {
                foreach(var i in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, i.Code);
                   
                }
                return View(registerVM);
            }
            TempData["success-notification"] = "Create Account Successfully, Please Confirm Your Email!";
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }
    }
}
