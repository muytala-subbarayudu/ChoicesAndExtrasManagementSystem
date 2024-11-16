using ChoicesExtrasManagement.Data;
using ChoicesExtrasManagement.Interfaces;
using ChoicesExtrasManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ChoicesExtrasManagement.Repository
{
    public class CatalogueRoomPlotTypeMappingRepository : ICatalogueRoomPlotTypeMappingRepository
    {
        private readonly ChoicesExtrasManagementDbContext _context;
        public CatalogueRoomPlotTypeMappingRepository(ChoicesExtrasManagementDbContext context)
        {
            _context = context;
        }

        public bool Add(CatalogueRoomPlotTypeMapping catalogueRoomPlotTypeMapping)
        {
            _context.Add(catalogueRoomPlotTypeMapping);
            return Save();
        }

        public bool Delete(CatalogueRoomPlotTypeMapping catalogueRoomPlotTypeMapping)
        {
            _context.Remove(catalogueRoomPlotTypeMapping);
            return Save();
        }

        public async Task<IEnumerable<CatalogueRoomPlotTypeMapping>> GetAllCatalogueMappingsByPlotTye(int plotTypeId)
        {
            return await _context.CatalogueRoomPlotTypeMappings.Include(c => c.Catalogue).Include(r => r.Room).Include(p=>p.PlotType).Where(i => i.PlotTypeId == plotTypeId).ToListAsync();
        }

        public async Task<IEnumerable<CatalogueRoomPlotTypeMapping>> GetAllCatalogueMappingsByPlotTyeAndRoom(int? plotTypeId, int? roomId)
        {
            return await _context.CatalogueRoomPlotTypeMappings.Include(c => c.Catalogue).Include(r => r.Room).Include(p => p.PlotType).Where(i => i.PlotTypeId == plotTypeId && i.RoomId == roomId).ToListAsync();
        }
        public async Task<IEnumerable<CatalogueRoomPlotTypeMapping>> GetCatalogueMappingsToDisplayByPlotTyeAndRoom(int? plotTypeId, int? roomId)
        {
            return await _context.CatalogueRoomPlotTypeMappings.Include(c => c.Catalogue).Include(r => r.Room).Include(p => p.PlotType).Where(i => i.PlotTypeId == plotTypeId && i.RoomId == roomId && i.IsActive).ToListAsync();
        }

        

        public async Task<IEnumerable<CatalogueRoomPlotTypeMapping>> GetCatalogueRoomPlotTypeMappings(int catalogueItemId)
        {
            return await _context.CatalogueRoomPlotTypeMappings.Include(c => c.Catalogue).Include(r=>r.Room).Include(p => p.PlotType).Where(i=>i.CatalogueId == catalogueItemId).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(CatalogueRoomPlotTypeMapping catalogueRoomPlotTypeMapping)
        {
            _context.Update(catalogueRoomPlotTypeMapping);
            return Save();
        }

    }
}
