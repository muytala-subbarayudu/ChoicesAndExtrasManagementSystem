using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChoicesExtrasManagement.Models
{
    public class SavedChoice
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Plot")]
        public int PlotId { get; set; }
        public Plot Plot { get; set; }
        [ForeignKey("PlotTypeRoomMapping")]
        public int PlotRoomId { get; set; }
        public PlotTypeRoomMapping PlotTypeRoomMapping { get; set; }
        [ForeignKey("Catalogue")]
        public int CatalogueId { get; set; }
        public Catalogue Catalogue { get; set; }
        [ForeignKey("SubCatalogue")]
        public int? SubCatalogueId { get; set; }
        public SubCatalogue? SubCatalogue { get; set; }
        public bool isSubmitted { get; set; }


    }
}
