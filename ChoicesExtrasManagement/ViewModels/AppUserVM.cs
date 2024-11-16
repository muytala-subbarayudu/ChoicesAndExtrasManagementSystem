using ChoicesExtrasManagement.Models;
using System.Collections;

namespace ChoicesExtrasManagement.ViewModels
{
    public class AppUserVM
    {
        public string Id { get; set; }  
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public int? PlotCount { get; set; }
        public IEnumerable<Plot> Plots { get; set; }
    }
}
