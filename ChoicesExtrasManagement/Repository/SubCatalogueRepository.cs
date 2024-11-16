using ChoicesExtrasManagement.Data;
using ChoicesExtrasManagement.Interfaces;
using ChoicesExtrasManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ChoicesExtrasManagement.Repository
{
    public class SubCatalogueRepository : ISubCatalogueRepository
    {
        private readonly ChoicesExtrasManagementDbContext _context;
        public SubCatalogueRepository(ChoicesExtrasManagementDbContext context)
        {
            _context = context;
        }

        public bool Add(SubCatalogue subCatalogueItem)
        {
            _context.Add(subCatalogueItem);
            return Save();
        }

        public bool Delete(SubCatalogue subCatalogueItem)
        {
            _context.Remove(subCatalogueItem);
            return Save();
        }

        public async Task<IEnumerable<SubCatalogue>> GetAllSubCatalogueItems(int catalogueItemId)
        {
            return await _context.SubCatalogues.Include(c => c.Catalogue).Where(i=>i.CatalogueId == catalogueItemId).ToListAsync();
        }

        public async Task<IEnumerable<SubCatalogue>> GetAllListSubCatalogueItems()
        {
            return await _context.SubCatalogues.Include(c => c.Catalogue).ToListAsync();
        }  

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(SubCatalogue subCatalogueItem)
        {
            _context.Update(subCatalogueItem);
            return Save();
        }
    }
}
