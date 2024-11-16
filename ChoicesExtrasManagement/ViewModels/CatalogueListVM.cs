using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.ViewModels
{
    public class CatalogueListVM
    {
        public int CatalogueId { get; set; }
        public string CatalogueName { get; set; }
        public bool ChoiceOrExtra { get; set; }
        public bool IsActive { get; set; }
    }
}
