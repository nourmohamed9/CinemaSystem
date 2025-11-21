
using CinemaSystem.DataAcess;
using CinemaSystem.Models;
using CinemaSystem.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;


namespace CinemaSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        public async Task<IActionResult> Index(Models.Movie movie)
        {
            // var model = _context.movies.Include(e => e.category).Include(e => e.cinemas).Include(e => e.actors).ToList();
            // var model = await _movieRepository.GetAsync(includes: [e => e.category, e => e.cinemas, e => e.actors]);
            var model = await _movieRepository.GetAsync(
                includes: new Expression<Func<Movie, object>>[] { e => e.category, e => e.cinemas, e => e.actors }
            );

            if (model == null) return NotFound();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            /* var movie = _context.movies
          .Include(e => e.category)
          .Include(e => e.cinemas)
          .Include(e => e.actors).AsNoTracking()
          .FirstOrDefault(e => e.ID == id);*/
            // var movie = await _movieRepository.GetOneAsync(expression: e => e.ID == id, includes: [e => e.category, e => e.cinemas, e => e.actors]);//
            var movie = await _movieRepository.GetOneAsync(
               e => e.ID == id,
               includes: new Expression<Func<Movie, object>>[] { e => e.category, e => e.cinemas, e => e.actors });
            if (movie == null) return NotFound();

           /* var viewModel = new indexVm
            {
                movies = new List<Movie> { movie }, // خاصية واحدة بدل movies
                categories = _context.categories.ToList(),
                cinemas = _context.cinemas.ToList(),
                actors = _context.actors.ToList()
            };*/
           
            var viewModel = new indexVm
            {
                movies = new List<Movie> { movie },
                categories = (await _categoryRepository.GetAsync()).ToList(),
                cinemas = (await _CinemaRepository.GetAsync()).ToList(),
                actors = (await _ActorRepository.GetAsync()).ToList()
            };
            return View(viewModel);
        }
        /*  [HttpPost]
          public IActionResult Edit(int id, Movie movie, IFormFile formFile, List<IFormFile> subimgs)
          {
              var movies = _context.movies
           .Include(e => e.category)
           .Include(e => e.cinemas)
           .Include(e => e.actors)
           .FirstOrDefault(e => e.ID == id);
              if (formFile != null)
              {
                  var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                  var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movie", fileName);

                  using (var steam = System.IO.File.Create(filePath))
                  {
                      formFile.CopyTo(steam);
                  }
                  var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movie", movies.MainImg);

                  if (System.IO.File.Exists(oldFilePath))
                      System.IO.File.Delete(oldFilePath);
                  movies.MainImg = fileName;
              }
              else
              {
                  movies.MainImg = movies.MainImg;//

              }
              _context.movies.Update(movie);
              _context.SaveChanges();
              if (subimgs != null)
              {
                  var movieSubImg = _context.movies.AsNoTracking().FirstOrDefault(e => e.ID == id);
                  foreach (var i in subimgs)
                  {
                      var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                      var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movie", fileName);
                      using (var steam = System.IO.File.Create(filePath))
                      {
                          i.CopyTo(steam);//
                      }

                      _context.movies.Add(movieSubImg);
                  }
                  foreach (var i in subimgs)
                  {
                      var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movie", movies.MainImg);

                      if (System.IO.File.Exists(oldFilePath))
                          System.IO.File.Delete(oldFilePath);
                      _context.movies.Remove(movieSubImg);
                  }
                  _context.SaveChanges();

              }
              _context.SaveChanges();
              return RedirectToAction("Index");
          }

      }*/
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Movie movie, IFormFile? formFile, List<IFormFile>? subimgs)
        {
            // جلب الفيلم من قاعدة البيانات مع جميع العلاقات
           /* var movieInDb = _context.movies
                .Include(m => m.category)
                .Include(m => m.cinemas)
                .Include(m => m.actors)
                .FirstOrDefault(m => m.ID == id);*/
          //  var movieInDb = await _movieRepository.GetOneAsync(expression: e => e.ID == id, includes: [e => e.category, e => e.cinemas, e => e.actors]);//
            var movieInDb = await _movieRepository.GetOneAsync(
              e => e.ID == id,
              includes: new Expression<Func<Movie, object>>[] { e => e.category, e => e.cinemas, e => e.actors }
          );
            if (movieInDb == null)
                return NotFound();

            // تحديث الصورة الرئيسية إذا تم رفع صورة جديدة
            if (formFile != null && formFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movie", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    formFile.CopyTo(stream);
                }

                // حذف الصورة القديمة
                if (!string.IsNullOrEmpty(movieInDb.MainImg))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movie", movieInDb.MainImg);
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
                }

                movieInDb.MainImg = fileName;
            }

            // تحديث البيانات الأساسية
            movieInDb.Name = movie.Name;
            movieInDb.Description = movie.Description;
            movieInDb.Status = movie.Status;
            movieInDb.Date = movie.Date;
            movieInDb.Time = movie.Time;
            movieInDb.CategoryId = movie.CategoryId;
            movieInDb.cinemaId = movie.cinemaId;
            movieInDb.ActorId = movie.ActorId;

            //_context.SaveChanges();
          await  _movieRepository.CommitAsync();

            // تحديث الصور الفرعية إذا تم رفع صور جديدة
            if (subimgs != null && subimgs.Count > 0)
            {
                foreach (var sub in subimgs)
                {
                    var subFileName = Guid.NewGuid().ToString() + Path.GetExtension(sub.FileName);
                    var subFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movie", subFileName);

                    using (var stream = System.IO.File.Create(subFilePath))
                    {
                        sub.CopyTo(stream);
                    }

                    // إذا عندك جدول MovieSubImages
                    //_context.MovieSubImages.Add(new MovieSubImage
                    //{
                    //    MovieId = id,
                    //    Img = subFileName
                    //});
                }
                //_context.SaveChanges();
             await   _movieRepository.CommitAsync();
            }

            return RedirectToAction("Index");
        }
        /* public IActionResult Delete(int ID)
         {
             var movie = _context.movies.FirstOrDefault(e => e.ID == ID);
             var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movie", movie.MainImg);
             if (System.IO.File.Exists(oldPath))
             {
                 System.IO.File.Delete(oldPath);
             }

             _context.movies.Remove(movie);
             _context.SaveChanges();


             return RedirectToAction("Index");
         }*/
        public async Task<IActionResult> Delete(int ID)
        {
            // 1. جلب الفيلم
          //  var movie = _context.movies.FirstOrDefault(e => e.ID == ID);
            var movie = await _movieRepository.GetOneAsync(e=>e.ID == ID);
            if (movie == null)
                return NotFound(); // لو الفيلم مش موجود

            // 2. حذف أي حجوزات مرتبطة بالفيلم
           //var bookings = _context.bookings.Where(b => b.MovieId == ID).ToList();
            var bookings =await  _bookingRepository.GetAsync(e => e.MovieId == ID);
            foreach (var b in bookings)
            {
               // _context.bookings.Remove(b);
                _bookingRepository.DeleteAsync(b);
            }

            // 3. حذف الصورة الرئيسية من السيرفر
            if (!string.IsNullOrEmpty(movie.MainImg))
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movie", movie.MainImg);
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            // 4. حذف الفيلم نفسه
           // _context.movies.Remove(movie);
           // _movieRepository.DeleteAsync(movie);
            // 5. حفظ التغييرات
           // _context.SaveChanges();
            await _bookingRepository.CommitAsync();
            await _movieRepository.CommitAsync();
            return RedirectToAction("Index");
        }
        public IActionResult DashBoard()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            //var movie = _context.movies.Include(e => e.category).AsEnumerable().FirstOrDefault(e => e.ID == id);
          //  var movie = await _movieRepository.GetOneAsync(expression: e => e.ID == id, includes: [e=>e.category]);
            var movie = await _movieRepository.GetOneAsync(
                e => e.ID == id,
                includes: new Expression<Func<Movie, object>>[] { e => e.category }
            );
            if (movie == null) return NotFound();
            /*var index = new indexVm
            {
                movies = new List<Movie> { movie },
                categories = _context.categories.ToList()

            };*/
            return View(movie.category);//
        }
     
        [HttpPost]
        public async Task<IActionResult> EditCategory(int id, Category categoryForm)
        {
            //  
            //var category = _context.categories.FirstOrDefault(c => c.ID == id);
            var category = await _categoryRepository.GetOneAsync(expression: c => c.ID == id);
            if (category == null) return NotFound();

            //
            category.Name = categoryForm.Name;
            category.Description = categoryForm.Description;

           // _context.SaveChanges(); // 
           await _categoryRepository.CommitAsync();
            // 
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditCinema(int id )
        {
            //var c = _context.movies.Include(e=>e.cinemas).FirstOrDefault(c => c.ID == id);
            //var c =await  _movieRepository.GetOneAsync(expression: c => c.ID == id, includes: [e => e.cinemas]);
            var c = await _movieRepository.GetOneAsync(
              e => e.ID == id,
              includes: new Expression<Func<Movie, object>>[] { e => e.cinemas }
          );
            if (c == null) return NotFound();
            return View(c.cinemas);

        }
        [HttpPost]
        public async Task<IActionResult> EditCinema(int id,Cinema cinema, IFormFile? Img)
        {

            //var c = _context.cinemas.FirstOrDefault(c => c.ID == id);
            var c = await _CinemaRepository.GetOneAsync(expression: c => c.ID == id);
            if (c == null) return NotFound();
            c.Name = cinema.Name;
            c.Location = cinema.Location;
            if (Img is not null && Img.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString()+Path.GetExtension( Img.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinema", fileName);
                using (var steam = System.IO.File.Create(filePath)) {
                    Img.CopyTo(steam);
                    
                };
                if (!string.IsNullOrEmpty(c.ImageUrl))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinema", c.ImageUrl);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }
                c.ImageUrl = fileName;

            }
            else
            {
                c.ImageUrl = cinema.ImageUrl;
            }
               // _context.SaveChanges();
          await  _categoryRepository.CommitAsync();
            return RedirectToAction("Index");
        }


        }
}
