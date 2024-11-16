using System.ComponentModel.DataAnnotations;

namespace ChoicesExtrasManagement.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email Address required")]
        [Display(Name = "Email Address")]
        public string? UserEmail { get; set; }
        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
