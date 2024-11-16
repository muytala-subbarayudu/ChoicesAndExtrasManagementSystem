using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChoicesExtrasManagement.Models
{
    public class PlotTypeRoomMapping
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Room")]
        public int? RoomId { get; set; }
        public Room? Room { get; set; }
        [ForeignKey("PlotType")]
        public int? PlotTypeId { get; set; }
        public PlotType? PlotType { get; set; }
        public string? DisplayName { get; set; } // Update Record Later
    }
}
