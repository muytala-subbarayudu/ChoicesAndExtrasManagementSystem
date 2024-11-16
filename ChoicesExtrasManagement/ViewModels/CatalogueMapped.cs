using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.ViewModels
{
    public class CatalogueMapped
    {
        public Catalogue Catalogue { get; set; }

        public IEnumerable<SubCatalogue> SubCatalogues { get; set; }

        public int? SubCatalogueMapped { get; set; }

        public int? SubCatalogueItemCount { get; set; }
        public bool isDisplayed { get; set; }

        public string SingleOrMultipleDisplayText { get { return ((this.Catalogue.SingleOrMultiple) ? "Yes" : "No"); } }

    }
}
