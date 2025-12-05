using CinemaSystem.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinemaSystem.Areas.Identity.Controllers
{
    [Area ("Identity")]
    [Authorize]

    public class ProfileController : Controller
        
    {
        UserManager<ApplicationUser> _UserManager;
        public ProfileController(UserManager<ApplicationUser> UserManager)
        {
            _UserManager = UserManager;

        }
        public async Task<IActionResult> Index()
        {
            var user = await _UserManager.GetUserAsync(User);
            UserInfoVM userInfoVM = user.Adapt<UserInfoVM>();
            return View(userInfoVM);
           
        }
        public async Task<IActionResult> UpdateInfo(UserInfoVM userInfoVM)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");
            var user = await _UserManager.GetUserAsync(User);
            user.Name = userInfoVM.Name;
            user.Email = userInfoVM.Email;
            user.Address = userInfoVM.Address;
            user.PhoneNumber = userInfoVM.PhoneNumber;
            await _UserManager.UpdateAsync(user);
            TempData["success-notification"] = "Update Profile Successfully";

            return RedirectToAction("Index");




        }
        public async Task<IActionResult> UpdatePassword(string CurrentPassword, string NewPassword)
        {
            if(CurrentPassword is null && NewPassword is null)
            {
                TempData["error-notification"] = "You must write CurrentPassword & NewPassword";
                return RedirectToAction("Index");
            }
            var user = await _UserManager.GetUserAsync(User);
        var result=   await _UserManager.ChangePasswordAsync(user, CurrentPassword, NewPassword);
            if (!result.Succeeded)
            {
                TempData["error-notification"] = String.Join(", ", result.Errors.Select(e => e.Code));
                return RedirectToAction("Index");
            }

            TempData["success-notification"] = "Update Password Successfully";
            return RedirectToAction("Index");


        }
    }
}
