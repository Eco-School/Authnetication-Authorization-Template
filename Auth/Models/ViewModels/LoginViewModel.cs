using System.ComponentModel.DataAnnotations;

namespace Auth.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }

        public string  ReturnUrl { get; set; }
    }
}