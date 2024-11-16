using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.Interfaces
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetAllLocations();
        bool Add(Location location);
        bool Update(Location location);
        bool Delete(Location location);
        bool Save();
    }
}
