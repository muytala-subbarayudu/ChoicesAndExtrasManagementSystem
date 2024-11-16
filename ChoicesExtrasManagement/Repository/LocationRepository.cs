using ChoicesExtrasManagement.Data;
using ChoicesExtrasManagement.Interfaces;
using ChoicesExtrasManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ChoicesExtrasManagement.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ChoicesExtrasManagementDbContext _context;
        public LocationRepository(ChoicesExtrasManagementDbContext context)
        {
            _context = context;
        }

        public bool Add(Location location)
        {
            _context.Add(location);
            return Save();
        }

        public bool Delete(Location location)
        {
            _context.Remove(location);
            return Save();
        }

        public async Task<IEnumerable<Location>> GetAllLocations()
        {
            return await _context.Locations.ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Location location)
        {
            _context.Update(location);
            return Save();
        }
    }
}
