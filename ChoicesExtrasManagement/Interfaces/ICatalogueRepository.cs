using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.Interfaces
{
    public interface ICatalogueRepository
    {
        Task<IEnumerable<Catalogue>> GetAllCatalogueItems();
        Task<IEnumerable<Catalogue>> GetAllCatalogues();
        Task<Catalogue> GetCatalogueItem(int catalogueId);
        bool Add(Catalogue catalogueItem);
        bool Update(Catalogue catalogueItem);
        bool Delete(Catalogue catalogueItem);
        bool Save();
    }
}
