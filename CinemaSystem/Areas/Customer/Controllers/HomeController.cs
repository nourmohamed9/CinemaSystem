
using CinemaSystem.DataAcess;
using CinemaSystem.Models;
using CinemaSystem.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Tasks;

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
        public HomeController(IRepository<Cinema> CinemaRepository, IRepository<Actor> ActorRepository, IRepository<Category> categoryRepository, IRepository<Movie> movieRepository, IRepository<Booking> bookingRepository)
        {
            _ActorRepository = ActorRepository;
            _categoryRepository = categoryRepository;
            _movieRepository = movieRepository;
            _CinemaRepository = CinemaRepository;
            _bookingRepository = bookingRepository;
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
        public async Task<IActionResult> Book(int id, indexVm indexVM, string customerName, int tickets)
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
            await _bookingRepository.CreateAsync(book);
            await _bookingRepository.CommitAsync();

            if (movies == null) return NotFound();
            return RedirectToAction("Index", "Home");
        }
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