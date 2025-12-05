using CinemaSystem.Models;
using CinemaSystem.Utilies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DiaSymReader;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CinemaSystem.Areas.Admin.Controllers
{
    [Area ("Admin")]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE}")]
    public class UserController : Controller
    {
        UserManager<ApplicationUser> _UserManager;
      public UserController(UserManager<ApplicationUser> UserManager)
        {
            _UserManager = UserManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _UserManager.Users.AsNoTracking().AsQueryable();
            return View(users);
        }
        public async Task<IActionResult> LockUnLock(string id)
        {
         var user= await _UserManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();
            user.LockoutEnabled = !user.LockoutEnabled;
            if (!user.LockoutEnabled && user.LockoutEnd > DateTime.UtcNow)//
            {
                user.LockoutEnd = DateTime.UtcNow.AddDays(30);
            }
            else
                user.LockoutEnd = null;
           await _UserManager.UpdateAsync(user);
                return RedirectToAction(nameof(Index));
        }
    }
}
