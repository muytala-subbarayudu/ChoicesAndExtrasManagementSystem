using ChoicesExtrasManagement.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChoicesExtrasManagement.ViewModels
{
    public class ProjectPlotsViewModel
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("Location")]
        public int? LocationId { get; set; }
        public Location? Location { get; set; }

        public IEnumerable<Plot> PlotsList;
    }
}
