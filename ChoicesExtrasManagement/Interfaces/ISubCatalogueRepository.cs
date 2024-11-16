using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.Interfaces
{
    public interface ISubCatalogueRepository
    {
        Task<IEnumerable<SubCatalogue>> GetAllSubCatalogueItems(int catalogueItemId);
        Task<IEnumerable<SubCatalogue>> GetAllListSubCatalogueItems();
        
        bool Add(SubCatalogue subCatalogueItem);
        bool Update(SubCatalogue subCatalogueItem);
        bool Delete(SubCatalogue subCatalogueItem);
        bool Save();
    }
}
