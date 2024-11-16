using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.ViewModels
{
    public class PlotTypesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string RoomDetails { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public IEnumerable<Room> AllRooms { get; set; }
    }
}
