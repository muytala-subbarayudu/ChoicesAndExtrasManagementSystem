using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChoicesExtrasManagement.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Plot")]
        public int PlotId { get; set; }
        public Plot Plot { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public DateTime? MessageDate { get; set; }
    }
}
