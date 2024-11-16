using ChoicesExtrasManagement.Data;
using ChoicesExtrasManagement.Interfaces;
using ChoicesExtrasManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ChoicesExtrasManagement.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ChoicesExtrasManagementDbContext _context;
        public RoomRepository(ChoicesExtrasManagementDbContext context)
        {
            _context = context;
        }

        public bool Add(Room room)
        {
            _context.Add(room);
            return Save();
        }

        public bool Delete(Room room)
        {
            _context.Remove(room);
            return Save();
        }

        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            return await _context.Rooms.ToListAsync();
        }

        public async Task<Room> GetRoomById(int? roomId)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r=>r.Id == roomId);
        }    

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Room room)
        {
            _context.Update(room);
            return Save();
        }
    }
}
