using ChoicesExtrasManagement.Data;
using ChoicesExtrasManagement.Interfaces;
using ChoicesExtrasManagement.Models;
using Microsoft.EntityFrameworkCore;
using Stripe.Terminal;

namespace ChoicesExtrasManagement.Repository
{
    public class SavedChoiceRepository : ISavedChoiceRepository
    {
        private readonly ChoicesExtrasManagementDbContext _context;
        public SavedChoiceRepository(ChoicesExtrasManagementDbContext context)
        {
            _context = context;
        }

        public bool Add(SavedChoice savedChoice)
        {
            _context.Add(savedChoice);
            return Save();
        }

        public bool Delete(SavedChoice savedChoice)
        {
            _context.Remove(savedChoice);
            return Save();
        }

        public async Task<IEnumerable<SavedChoice>> GetSavedChoicesByPlotRoom(int? plotId, int? plotRoomId)
        {
            return await _context.SavedChoices
                .Include(x => x.Plot)
                .Include(x => x.PlotTypeRoomMapping)
                .Include(x => x.Catalogue)
                .Include(x => x.SubCatalogue)
                .Where(c=>c.PlotId == plotId && c.PlotRoomId == plotRoomId)
                .ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(SavedChoice savedChoice)
        {
            _context.Update(savedChoice);
            return Save();
        }
    }
}
