using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.ViewModels
{
    public class PlotRoomVM
    {
        public Plot Plot  { get; set; }
        public List<RoomVM> RoomVMs { get; set; }
    }
}
