using ChoicesExtrasManagement.Data;
using ChoicesExtrasManagement.Interfaces;
using ChoicesExtrasManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ChoicesExtrasManagement.Repository
{
    public class PlotTypeRoomMappingRepository : IPlotTypeRoomMappingRepository
    {

        private readonly ChoicesExtrasManagementDbContext _context;
        public PlotTypeRoomMappingRepository(ChoicesExtrasManagementDbContext context)
        {
            _context = context;
        }

        public bool Add(PlotTypeRoomMapping plotTypeRoomMapping)
        {
            _context.Add(plotTypeRoomMapping);
            return Save();
        }

        public bool Delete(PlotTypeRoomMapping plotTypeRoomMapping)
        {
            _context.Remove(plotTypeRoomMapping);
            return Save();
        }

        public async Task<IEnumerable<PlotTypeRoomMapping>> GetRoomMappingsByPlotType(int? plotTypeId)
        {
            return await _context.PlotTypeRoomMappings.Include(r=>r.Room).Include(pt => pt.PlotType).Where(i => i.PlotTypeId == plotTypeId).ToListAsync();
        }

        public async Task<PlotTypeRoomMapping> GetRoomPlotTypeById(int? roomPappingId)
        {
            return await _context.PlotTypeRoomMappings.Include(r => r.Room).Include(pt => pt.PlotType).Where(i => i.Id == roomPappingId).FirstOrDefaultAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(PlotTypeRoomMapping plotTypeRoomMapping)
        {
            _context.Update(plotTypeRoomMapping);
            return Save();
        }
    }
}
