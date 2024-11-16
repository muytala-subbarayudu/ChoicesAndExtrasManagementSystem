using Microsoft.AspNetCore.Identity;

namespace ChoicesExtrasManagement.Models
{
    public class AppUser : IdentityUser
    {
        public DateTime? RegisteredDate { get; set; }

    }
}
