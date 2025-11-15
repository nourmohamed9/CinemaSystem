using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaSystem.Areas.Admin.Models
{
    public class Movie
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string? MainImg { get; set; }
        public string? SubImg { get; set; }

        public int ActorId { get; set; }
        public List<Actor>? actors { get; set; }
        [ForeignKey("Cinema")]
        public int cinemaId { get; set; }
        public Cinema? cinemas { get; set; }

        public int CategoryId { get; set; }
        public Category? category { get; set; }
    }
}
