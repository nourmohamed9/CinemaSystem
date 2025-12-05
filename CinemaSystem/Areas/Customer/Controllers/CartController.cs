
using CinemaSystem.Migrations;
using CinemaSystem.Models;
using CinemaSystem.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Cart = CinemaSystem.Models.Cart;

namespace CinemaSystem.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        IRepository<Movie> _movieRepository;
        IRepository<Cart> _cartRepository;
        IRepository<Promotion> _promotionRepository;
       public CartController(UserManager<ApplicationUser> userManager, IRepository<Movie> movieRepository, IRepository<Cart> cartRepository, IRepository<Promotion> promotionRepository)
        {
            _cartRepository = cartRepository;
            _movieRepository = movieRepository;
            _userManager = userManager;
            _promotionRepository = promotionRepository;
        }

        public async Task<IActionResult> AddToCart(int movieId, int count)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return NotFound();
            var movie = await _movieRepository.GetOneAsync(e => e.ID == movieId, tracked: false);
            if (movie is null) return NotFound();
            var cart = await _cartRepository.GetOneAsync(e => e.ApplicationUserId == user.Id && e.movieId == movie.ID);
            if (cart is not null)
                cart.Count += count;
            else
            {
               await _cartRepository.CreateAsync(new Cart
                {
                    movieId = movie.ID,
                    Count = count,
                    ApplicationUserId = user.Id,

                });
            
            }
            await _cartRepository.CommitAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Index(string code)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return NotFound();
            var cart =await _cartRepository.GetAsync(e => e.ApplicationUserId == user.Id, includes: [e=>e.movie]);
            if(code is not null)
            {
                var promotion = await _promotionRepository.GetOneAsync(e => e.Code == code && e.isValid && e.ValidTo > DateTime.UtcNow && e.MaxUsage > 0);
                if (promotion is null)
                    TempData["error-notification"] = "Invalid Code";
                else
                {
                    bool founded = false;
                    foreach(var i in cart)
                    {
                        if(i.movieId == promotion.MovieId)
                        {
                            i.Price -= (i.Price * (promotion.Discount / 100));
                            promotion.MaxUsage -= 1;

                            if (promotion.MaxUsage == 0)

                                promotion.isValid = false;

                            await _cartRepository.CommitAsync();
                            TempData["success-notification"] = "Apply Code";

                            founded = true;
                            break;
                        }
                    }
                    if (!founded)
                        TempData["error-notification"] = "Invalid Code";
                }




            }
            return View(cart);
        }
        public async Task<IActionResult> IncrementCount(int movieId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return NotFound();

            var cart = await _cartRepository.GetOneAsync(e => e.ApplicationUserId == user.Id && e.movieId ==movieId);
            if (cart is null) return NotFound();
            cart.Count += 1;
            await _cartRepository.CommitAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DecrementCount(int movieId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return NotFound();

            var cart = await _cartRepository.GetOneAsync(e => e.ApplicationUserId == user.Id && e.movieId == movieId);
            if (cart is null) return NotFound();
            if (cart.Count > 1)
            {
                cart.Count -= 1;
            }
            
            await _cartRepository.CommitAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteItem (int movieId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return NotFound();
            var cart = await _cartRepository.GetOneAsync(e => e.ApplicationUserId == user.Id && e.movieId == movieId);
          
            if(cart is null)
            {
                return NotFound();

            }
            _cartRepository.DeleteAsync(cart);
            await _cartRepository.CommitAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
