using CinemaSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models
{
    public class Booking
    {
        [Key]
        public int ID { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public int Tickets { get; set; }
    }
}
