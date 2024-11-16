using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChoicesExtrasManagement.Models
{
    public class SubCatalogue
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("Catalogue")]
        public int? CatalogueId { get; set; }
        public Catalogue? Catalogue { get; set; }

    }
}
