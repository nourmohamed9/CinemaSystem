using CinemaSystem.DataAcess;
using CinemaSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CinemaSystem.Utilies.DBInitializer
{
    public class DBInitializer:IDBInitializer
    {
        RoleManager<IdentityRole> _RoleManager;
        UserManager<ApplicationUser> _userManager;
        ApplicationDbContext _context;
        public DBInitializer(RoleManager<IdentityRole> RoleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _RoleManager = RoleManager;
            _userManager = userManager;
            _context = context;
        }
        public void Initialize()
        {
            if (_context.Database.GetPendingMigrations().Any())
            {
                _context.Database.Migrate();
            }
            if (_RoleManager.Roles.IsNullOrEmpty())
            {
                _RoleManager.CreateAsync(new(SD.SUPER_ADMIN_ROLE)).GetAwaiter().GetResult();
                _RoleManager.CreateAsync(new(SD.ADMIN_ROLE)).GetAwaiter().GetResult();
                _RoleManager.CreateAsync(new(SD.EMPLOYEE_ROLE)).GetAwaiter().GetResult();
                _RoleManager.CreateAsync(new(SD.CUSTOMER_ROLE)).GetAwaiter().GetResult();

            }
            _userManager.CreateAsync(new()
            {
                Email = "superadmin@Cinema.Com",
                EmailConfirmed = true,
                UserName = "SuperAdmin",
                Name = "SuperAdmin"
            }, "Admin123$").GetAwaiter().GetResult();
            var user =_userManager.FindByNameAsync("SuperAdmin").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(user!, SD.SUPER_ADMIN_ROLE).GetAwaiter().GetResult();
        }
    }
}
