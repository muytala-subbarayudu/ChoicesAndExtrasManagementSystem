using ChoicesExtrasManagement.Data;
using ChoicesExtrasManagement.Interfaces;
using ChoicesExtrasManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ChoicesExtrasManagement.Repository
{
    public class PlotTypeRepository : IPlotTypeRepository
    {
        private readonly ChoicesExtrasManagementDbContext _context;
        public PlotTypeRepository(ChoicesExtrasManagementDbContext context)
        {
            _context = context;
        }

        public bool Add(PlotType plotType)
        {
            _context.Add(plotType);
            return Save();
        }

        public bool Delete(PlotType plotType)
        {
            _context.Remove(plotType);
            return Save();
        }

        public async Task<IEnumerable<PlotType>> GetAllPlotTypes()
        {
            return await _context.PlotTypes.ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(PlotType plotType)
        {
            _context.Update(plotType);
            return Save();
        }
    }
}
