using ChoicesExtrasManagement.Data;
using ChoicesExtrasManagement.Interfaces;
using ChoicesExtrasManagement.Models;
using Microsoft.EntityFrameworkCore;
using Stripe.Terminal;

namespace ChoicesExtrasManagement.Repository
{
    public class SavedExtraRepository : ISavedExtraRepository
    {
        private readonly ChoicesExtrasManagementDbContext _context;
        public SavedExtraRepository(ChoicesExtrasManagementDbContext context)
        {
            _context = context;
        }

        public bool Add(SavedExtra savedExtra)
        {
            _context.Add(savedExtra);
            return Save();
        }

        public bool Delete(SavedExtra savedExtra)
        {
            _context.Remove(savedExtra);
            return Save();
        }

        public async Task<IEnumerable<SavedExtra>> GetSavedExtrasByPlotRoom(int? plotId, int? plotRoomId)
        {
            return await _context.SavedExtras
                .Include(x => x.Plot)
                .Include(x => x.Plot.Project)
                .Include(x => x.Plot.PlotType)
                .Include(x => x.Plot.Project.Location)
                .Include(x => x.PlotTypeRoomMapping)
                .Include(x => x.Catalogue)
                .Include(x => x.SubCatalogue)
                .Include(x => x.Transaction)
                .Where(c => c.PlotId == plotId && c.PlotRoomId == plotRoomId)
                .ToListAsync();
        }
        public async Task<IEnumerable<SavedExtra>> GetSavedExtrasByPlot(int? plotId)
        {
            return await _context.SavedExtras
                .Include(x => x.Plot)
                .Include(x => x.Plot.Project)
                .Include(x => x.Plot.PlotType)
                .Include(x => x.Plot.Project.Location)
                .Include(x => x.PlotTypeRoomMapping)
                .Include(x => x.Catalogue)
                .Include(x => x.SubCatalogue)
                .Include(x => x.Transaction)
                .Where(c => c.PlotId == plotId)
                .ToListAsync();
        }

        public async Task<IEnumerable<SavedExtra>> GetSavedExtrasAsOrders()
        {
            return await _context.SavedExtras
                .Include(x => x.Plot)
                .Include(x => x.Plot.AppUser)
                .Include(x => x.Plot.Project)
                .Include(x => x.Plot.PlotType)
                .Include(x => x.Plot.Project.Location)
                .Include(x => x.PlotTypeRoomMapping)
                .Include(x => x.Catalogue)
                .Include(x => x.SubCatalogue)
                .Include(x => x.Transaction)
                .Where(c => c.TransactionId !=null)
                .ToListAsync();
        }

        public async Task<IEnumerable<SavedExtra>> GetSavedExtrasAsOutstanding(int? plotId)
        {
            return await _context.SavedExtras
                .Include(x => x.Plot)
                .Include(x => x.Plot.Project)
                .Include(x => x.Plot.PlotType)
                .Include(x => x.Plot.Project.Location)
                .Include(x => x.PlotTypeRoomMapping)
                .Include(x => x.Catalogue)
                .Include(x => x.SubCatalogue)
                .Include(x => x.Transaction)
                .Where(c => c.PlotId == plotId && c.TransactionId == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<SavedExtra>> GetSavedExtrasAsOutstandingRoom(int? plotId, int plotRoomId)
        {
            return await _context.SavedExtras
                .Include(x => x.Plot)
                .Include(x => x.Plot.Project)
                .Include(x => x.Plot.PlotType)
                .Include(x => x.Plot.Project.Location)
                .Include(x => x.PlotTypeRoomMapping)
                .Include(x => x.Catalogue)
                .Include(x => x.SubCatalogue)
                .Include(x => x.Transaction)
                .Where(c => c.PlotId == plotId && c.TransactionId == null && c.PlotRoomId == plotRoomId)
                .ToListAsync();
        }

        


        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(SavedExtra savedExtra)
        {
            _context.Update(savedExtra);
            return Save();
        }
    }
}
