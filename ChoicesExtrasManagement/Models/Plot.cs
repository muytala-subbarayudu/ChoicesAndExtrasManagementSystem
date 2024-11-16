using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChoicesExtrasManagement.Models
{
    public class Plot
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("PlotType")]
        public int? PlotTypeId { get; set; }
        public PlotType? PlotType { get; set; }
        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }



    }
}
