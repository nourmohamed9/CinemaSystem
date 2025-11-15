using CinemaSystem.Areas.Customer.Data;
using CinemaSystem.Areas.Customer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices.Marshalling;

namespace CinemaSystem.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public IActionResult Index(Movie movie,int page=1)
        {
            var itemPerPage = 3;
            var allmovies = _context.movies.AsNoTracking().ToList();
            var count = (int)Math.Ceiling((double)allmovies.Count() / itemPerPage);
            var mo = allmovies.Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();

            var model = new indexVm
            {
                // movies = _context.movies.ToList(),
                movies = mo,
                categories = _context.categories.ToList(),
                actors = _context.actors.ToList(),
                cinemas = _context.cinemas.ToList(),
                Count=count,
                CurrentPage=page

            };
            if (model == null) return NotFound();
            return View(model);
        }
        [HttpGet]
        public IActionResult Book(int id)
        {
            var movies = _context.movies.FirstOrDefault(e => e.ID == id);
            var model = new indexVm
            {
                movies = new List<Movie> { movies }, // حط الـ movie في قائمة لو indexVm متوقع List
                actors = _context.actors.ToList(),
                cinemas = _context.cinemas.ToList(),
                categories = _context.categories.ToList()
            };
            if (movies == null) return NotFound();
            return View(movies);
        }
        [HttpPost]
        public IActionResult Book(int id, indexVm indexVM, string customerName, int tickets)
        {
            var movies = _context.movies.Include(e => e.category).Include(e => e.actors).Include(e => e.cinemas).ToList().FirstOrDefault(e => e.ID == id);
             var model = new indexVm //
             {
                 movies = _context.movies.ToList(),
                 actors = _context.actors.ToList(),
                 cinemas = _context.cinemas.ToList(),
                 categories = _context.categories.ToList()
             };
            var book = new Booking
            {
                MovieId = id,
                Tickets = tickets,
                CustomerName = customerName
            };
            _context.bookings.Add(book);
            _context.SaveChanges();


            if (movies == null) return NotFound();
         return RedirectToAction("Index", "Home");
        }
        public IActionResult MyBookings( Movie movie, string customerName, int tickets)
        {
            var bookings = _context.bookings
       .Include(b => b.Movie)
           .ThenInclude(m => m.cinemas)
       .Include(b => b.Movie)
           .ThenInclude(m => m.category)
       .Include(b => b.Movie)
           .ThenInclude(m => m.actors)
       .ToList();
            return View(bookings);

        }
        
    }
}
