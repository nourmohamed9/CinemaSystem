namespace CinemaSystem.Areas.Customer.Models
{
    public class indexVm
    {
        public List<Actor>? actors { get; set; }
        public List<Cinema>? cinemas { get; set; }
        public List<Category>? categories { get; set; }
        public List<Movie>? movies { get; set; }
        public int Count { get; set; }
        public int CurrentPage { get; set; }
    }
}
