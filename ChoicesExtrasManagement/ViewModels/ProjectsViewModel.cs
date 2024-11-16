using ChoicesExtrasManagement.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChoicesExtrasManagement.ViewModels
{
    public class ProjectsViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Location> AllLocations { get; set; }
        public int? currentLocation { get; set; }
    }
}
