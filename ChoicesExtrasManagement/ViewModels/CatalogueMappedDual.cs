using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.ViewModels
{
    public class CatalogueMappedDual
    {
        public Catalogue Catalogue { get; set; }

        public IEnumerable<SubCatalogue> SubCatalogues { get; set; }

        public int? SubCatalogueMapped { get; set; }

        public int? SubCatalogueItemCount { get; set; }
        public bool isDisplayed { get; set; }

        public decimal? PricePaid { get; set; }

        public string? Transaction { get;set; }

        public bool YesOrNo { get; set; }

        public string SingleOrMultipleDisplayText { get { return ((this.Catalogue.SingleOrMultiple) ? "Yes" : "No"); } }
    }
}
