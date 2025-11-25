using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models
{
    public class ResendEmailConfirmationVM
    {
        public int Id { get; set; }

        [Required]
        public string UserNameOrEmail { get; set; } = string.Empty;
    }
}
