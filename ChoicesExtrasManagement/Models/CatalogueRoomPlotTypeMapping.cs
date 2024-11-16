using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChoicesExtrasManagement.Models
{
    public class CatalogueRoomPlotTypeMapping
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Catalogue")]
        public int? CatalogueId { get; set; }
        public Catalogue? Catalogue { get; set; }
        [ForeignKey("Room")]
        public int? RoomId { get; set; }
        public Room? Room { get; set; }
        [ForeignKey("PlotType")]
        public int? PlotTypeId { get; set; }
        public PlotType? PlotType { get; set; }
        public bool IsActive { get; set; }
        public bool ChoiceorExtra { get; set; }

    }
}
