using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChoicesExtrasManagement.Models
{
    public class SavedExtra
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
        public int SubCatalogueId { get; set; }
        public SubCatalogue SubCatalogue { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("PaymentTransaction")]
        public string? TransactionId { get; set; }
        public PaymentTransaction? Transaction { get; set; }
        public decimal? PricePer { get;set; }
        public decimal? AmountPaid { get; set; }
        public bool isSubmitted { get; set; }
    }
}
