using System.ComponentModel.DataAnnotations;

namespace ChoicesExtrasManagement.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name required")]
        [Display(Name = "Name")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Email Address required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Email Address")]
        public string? UserEmail { get; set; }
        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Phone Number required")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Not a valid phone number, 10 digits number required")]
        public string? PhoneNumber { get; set; }
    }
}
