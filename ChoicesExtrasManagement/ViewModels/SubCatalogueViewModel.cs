using ChoicesExtrasManagement.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChoicesExtrasManagement.ViewModels
{
    public class SubCatalogueViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int? CatalogueId { get; set; }
        public Catalogue? Catalogue { get; set; }
    }
}
