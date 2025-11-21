using CinemaSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaSystem.DataAcess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Actor> actors { get; set; }
        public DbSet<Cinema> cinemas { get; set; }
        public DbSet<Movie> movies { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Booking> bookings { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-EMOIHI8\\SQLEXPRESS;database=CinemaSys2;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
