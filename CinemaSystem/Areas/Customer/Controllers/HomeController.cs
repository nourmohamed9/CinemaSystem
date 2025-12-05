
using CinemaSystem.DataAcess;
using CinemaSystem.Migrations;
using CinemaSystem.Models;
using CinemaSystem.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Tasks;
using Cart = CinemaSystem.Models.Cart;


namespace CinemaSystem.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        ApplicationDbContext _context;// = new ApplicationDbContext();
        private IRepository<Cinema> _CinemaRepository;
        private IRepository<Actor> _ActorRepository;
        private IRepository<Category> _categoryRepository;
        private IRepository<Movie> _movieRepository;
        private IRepository<Booking> _bookingRepository;
        private IRepository<Promotion> _promotionRepository;

        UserManager<ApplicationUser> _userManager;
        IRepository<Cart> _cartRepository;
        public HomeController(IRepository<Cinema> CinemaRepository, IRepository<Actor> ActorRepository, IRepository<Category> categoryRepository, IRepository<Movie> movieRepository, IRepository<Booking> bookingRepository, IRepository<Promotion> promotionRepository, UserManager<ApplicationUser> userManager, IRepository<Cart> cartRepository)
        {
            _ActorRepository = ActorRepository;
            _categoryRepository = categoryRepository;
            _movieRepository = movieRepository;
            _CinemaRepository = CinemaRepository;
            _bookingRepository = bookingRepository;
            _promotionRepository = promotionRepository;
            _userManager = userManager;
            _cartRepository = cartRepository;
        }

        public async Task<IActionResult> Index(Movie movie, int page = 1)
        {
            var itemPerPage = 3;
          //  var allmovies = _context.movies.AsNoTracking().ToList();
            var allmovies = await _movieRepository.GetAsync(tracked:false);
            var count = (int)Math.Ceiling((double)allmovies.Count() / itemPerPage);
            var mo = allmovies.Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();

            var model = new indexVm
            {
                // movies = _context.movies.ToList(),
                movies = mo.ToList(),
                categories = (List<Category>)await _categoryRepository.GetAsync(),
                actors = (List<Actor>)await _ActorRepository.GetAsync(),
                cinemas = (List<Cinema>)await _CinemaRepository.GetAsync(),
                Count = count,
                CurrentPage = page

            };
            if (model == null) return NotFound();
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieRepository.GetOneAsync(e => e.ID == id,includes:new Expression<Func<Movie, object>>[]
            {
                e=>e.cinemas,
                e=>e.category,
                e=>e.actors
            }, tracked:false);//

            if (movie is null)
                return NotFound();
           
            return View(movie
            
               
            );
        }
        [HttpGet]
        public async Task<IActionResult> Book(int id)
        {
            //var movies = _context.movies.FirstOrDefault(e => e.ID == id);
            var movies =await _movieRepository.GetOneAsync(expression: e => e.ID == id);
           var model = new indexVm
            {
                movies = new List<Movie> { movies }, // حط الـ movie في قائمة لو indexVm متوقع List
                actors = (List<Actor>)await _ActorRepository.GetAsync(),
                cinemas = (List<Cinema>)await _CinemaRepository.GetAsync(),
                categories = (List<Category>)await _categoryRepository.GetAsync()
           };
            if (movies == null) return NotFound();
            return View(movies);
        }
        [HttpPost]
        public async Task<IActionResult> Book(int id, indexVm indexVM, string customerName, int tickets, string code)
        {
            //var movies = _context.movies.Include(e => e.category).Include(e => e.actors).Include(e => e.cinemas).ToList().FirstOrDefault(e => e.ID == id);
            var movies = await _movieRepository.GetOneAsync(
    expression: e => e.ID == id,
    includes: new Expression<Func<Movie, object>>[]
    {
        e => e.category,
        e => e.actors,
        e => e.cinemas
    }
);
            var model = new indexVm //
            {
                movies = (List<Movie>)await _movieRepository.GetAsync(),
                actors = (List<Actor>)await _ActorRepository.GetAsync(),
                cinemas = (List<Cinema>)await _CinemaRepository.GetAsync(),
                categories = (List<Category>)await _categoryRepository.GetAsync()
            };
            var book = new Booking
            {
                MovieId = id,
                Tickets = tickets,
                CustomerName = customerName
            };
            // _context.bookings.Add(book);
            // _context.SaveChanges();
          
            //

            var user = await _userManager.GetUserAsync(User);
            if (user is null) return NotFound();
            var cart = await _cartRepository.GetAsync(e => e.ApplicationUserId == user.Id, includes: [e => e.movie]);
            if (code is not null)
            {

                var promotion = await _promotionRepository.GetOneAsync(e => e.Code == code && e.isValid && e.ValidTo > DateTime.UtcNow && e.MaxUsage > 0);
                if (promotion is null)
                    TempData["error-notification"] = "Invalid Code";
                else
                {
                    bool founded = false;
                    foreach (var i in cart)
                    {
                        if (i.movieId == promotion.MovieId)
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


            await _bookingRepository.CreateAsync(book);
            await _bookingRepository.CommitAsync();


            if (movies == null) return NotFound();
                return RedirectToAction("Index", "Home");
            }
        
       /* public async Task<IActionResult> Cart(string code)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return NotFound();
            var cart = await _cartRepository.GetAsync(e => e.ApplicationUserId == user.Id, includes: [e => e.movie]);
            if (code is not null)
            {

                var promotion = await _promotionRepository.GetOneAsync(e => e.Code == code && e.isValid && e.ValidTo > DateTime.UtcNow && e.MaxUsage > 0);
                if (promotion is null)
                    TempData["error-notification"] = "Invalid Code";
                else
                {
                    bool founded = false;
                    foreach (var i in cart)
                    {
                        if (i.movieId == promotion.MovieId)
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
        }*/
        public async Task<IActionResult> MyBookings(Movie movie, string customerName, int tickets)
        {
            /* var bookings = _context.bookings
        .Include(b => b.Movie)
            .ThenInclude(m => m.cinemas)
        .Include(b => b.Movie)
            .ThenInclude(m => m.category)
        .Include(b => b.Movie)
            .ThenInclude(m => m.actors)
        .ToList();*/
            var bookings = await _bookingRepository.GetAsync(
            includes: new Expression<Func<Booking, object>>[]
            {
            b => b.Movie,
            b => b.Movie.category,
            b => b.Movie.cinemas,
            b => b.Movie.actors
            },
            tracked: false
        );
            return View(bookings);

        }


    }
}