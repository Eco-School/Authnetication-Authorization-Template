using System.ComponentModel.DataAnnotations;

namespace Auth.Models.ViewModels
{
    public class UserRegisterViewModel
    {
        [Required]
        public string FullName { get; set; }
        [Required, RegularExpression(@"^(90|91|92|93|98|50|55|88|77|99|94|70|00|11|20)[\d]{7}$",
             ErrorMessage = "Неправилный формат номера")]
        public string PhoneNumber { get; set; }

        [Required, EmailAddress]
        public string  Email { get; set; }
        
        [Required,Display(Name = "Password"),DataType(DataType.Password)]
        public string Password { get; set; }
        [Required,Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Подтвердите пароль")]
        public string ConfirmPassword { get; set; }  
    }
}