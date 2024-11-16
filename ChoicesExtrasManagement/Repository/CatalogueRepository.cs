using ChoicesExtrasManagement.Data;
using ChoicesExtrasManagement.Interfaces;
using ChoicesExtrasManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ChoicesExtrasManagement.Repository
{
    public class CatalogueRepository : ICatalogueRepository
    {
        private readonly ChoicesExtrasManagementDbContext _context;
        public CatalogueRepository(ChoicesExtrasManagementDbContext context)
        {
            _context = context;
        }

        public bool Add(Catalogue catalogueItem)
        {
            _context.Add(catalogueItem);
            return Save();
        }

        public bool Delete(Catalogue catalogueItem)
        {
            _context.Remove(catalogueItem);
            return Save();
        }

        public async Task<IEnumerable<Catalogue>> GetAllCatalogueItems()
        {
            return await _context.Catalogues.ToListAsync();
        }

        public async Task<Catalogue> GetCatalogueItem(int catalogueId)
        {
            return await _context.Catalogues.Where(c=>c.Id ==  catalogueId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Catalogue>> GetAllCatalogues()
        {
            return await _context.Catalogues.ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Catalogue catalogueItem)
        {
            _context.Update(catalogueItem);
            return Save();
        }
    }
}
