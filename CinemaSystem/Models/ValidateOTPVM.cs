using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models
{
    public class ValidateOTPVM
    {
        public int Id { get; set; }
        public  string UserId { get; set; } = string.Empty;
        [Required]
        public string OTP { get; set; } = string.Empty;


    }
}
