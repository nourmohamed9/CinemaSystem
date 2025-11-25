
using CinemaSystem.DataAcess;
using CinemaSystem.Models;
using CinemaSystem.Repositories.IRepositories;
using CinemaSystem.Repository;
using CinemaSystem.Utilies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
namespace CinemaSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString =
                  builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("Connection string"
                      + "'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddControllersWithViews();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(confi =>
            {
                confi.User.RequireUniqueEmail = true;
                confi.Password.RequiredLength = 7;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders(); 

            // Authorization
            builder.Services.AddAuthorization();

            // Add services to the container.
            /*  builder.Services.AddControllersWithViews();
              builder.Services.AddScoped<IRepository<Movie>, Repository<Movie>>();
              builder.Services.AddScoped<IRepository<Cinema>, Repository<Cinema>>();
              builder.Services.AddScoped<IRepository<Actor>, Repository<Actor>>();
              builder.Services.AddScoped<IRepository<Booking>, Repository<Booking>>();
              builder.Services.AddScoped<IRepository<Category>, Repository<Category>>();*/

            builder.Services.AddScoped<IRepository<CinemaSystem.Models.Movie>, Repository<CinemaSystem.Models.Movie>>();
            builder.Services.AddScoped<IRepository<CinemaSystem.Models.Cinema>, Repository<CinemaSystem.Models.Cinema>>();
            builder.Services.AddScoped<IRepository<CinemaSystem.Models.Actor>, Repository<CinemaSystem.Models.Actor>>();
            builder.Services.AddScoped<IRepository<CinemaSystem.Models.Category>, Repository<CinemaSystem.Models.Category>>();
            builder.Services.AddScoped<IRepository<Booking>, Repository<Booking>>();

            builder.Services.AddScoped<IRepository<CinemaSystem.Models.Movie>, Repository<CinemaSystem.Models.Movie>>();
            builder.Services.AddScoped<IRepository<CinemaSystem.Models.Cinema>, Repository<CinemaSystem.Models.Cinema>>();
            builder.Services.AddScoped<IRepository<Models.Actor>, Repository<Models.Actor>>();
            builder.Services.AddScoped<IRepository<CinemaSystem.Models.Category>, Repository<CinemaSystem.Models.Category>>();
            builder.Services.AddScoped<IRepository<CinemaSystem.Models.Booking>, Repository<CinemaSystem.Models.Booking>>();

            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IRepository<ApplicationUserOTP>, Repository< ApplicationUserOTP>>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
     name: "areas",
     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
