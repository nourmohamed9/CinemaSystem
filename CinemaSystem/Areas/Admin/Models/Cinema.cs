namespace CinemaSystem.Areas.Admin.Models
{
    public class Cinema
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string? ImageUrl { get; set; }
    }
}
