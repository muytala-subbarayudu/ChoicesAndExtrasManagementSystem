using ChoicesExtrasManagement.Models;
using ChoicesExtrasManagement.ViewModels;

namespace ChoicesExtrasManagement.Interfaces
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllRooms();
        Task<Room> GetRoomById(int? roomId);
        bool Add(Room room);
        bool Update(Room room);
        bool Delete(Room room);
        bool Save();
    }
}
