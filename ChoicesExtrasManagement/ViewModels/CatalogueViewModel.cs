using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.ViewModels
{
    public class CatalogueViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<SubCatalogue> SubCatalogueItems { get; set; }
        public bool SingleOrMultiple { get; set; }
    }
}
