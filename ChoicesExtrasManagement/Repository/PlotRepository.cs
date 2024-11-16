using ChoicesExtrasManagement.Data;
using ChoicesExtrasManagement.Interfaces;
using ChoicesExtrasManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ChoicesExtrasManagement.Repository
{
    public class PlotRepository : IPlotRepository
    {
        private readonly ChoicesExtrasManagementDbContext _context;
        public PlotRepository(ChoicesExtrasManagementDbContext context)
        {
            _context = context;
        }

        public bool Add(Plot plot)
        {
            _context.Add(plot);
            return Save();
        }

        public bool Delete(Plot plot)
        {
            _context.Remove(plot);
            return Save();
        }

        public Plot GetPlot(int plotId)
        {
            return _context.Plots.Include(p => p.PlotType).Include(p => p.Project).Include(u => u.AppUser).FirstOrDefault(i => i.Id == plotId);
        }


        public async Task<IEnumerable<Plot>> GetAllPlotsByProject(int projectId)
        {
            return await _context.Plots.Include(p=> p.PlotType).Include(p=>p.Project).Include(u=>u.AppUser).Where(i => i.ProjectId == projectId).ToListAsync();
        }


        public async Task<IEnumerable<Plot>> GetAllPlotsByPlotType(int plotTypeId)
        {
            return await _context.Plots.Include(p => p.PlotType).Include(p => p.Project).Include(u => u.AppUser).Where(i => i.PlotTypeId == plotTypeId).ToListAsync();

        }
        public async Task<IEnumerable<Plot>> GetAllPlotsByUser(string userId)
        {
            return await _context.Plots
                .Include(p => p.PlotType)
                .Include(p => p.Project)
                .Include(u=>u.AppUser)
                .Include(l=>l.Project.Location)
                .Where(i => i.AppUserId == userId).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Plot plot)
        {
            _context.Update(plot);
            return Save();
        }
    }
}
