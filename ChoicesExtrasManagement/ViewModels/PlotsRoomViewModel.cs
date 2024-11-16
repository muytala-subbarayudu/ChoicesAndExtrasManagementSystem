using ChoicesExtrasManagement.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChoicesExtrasManagement.ViewModels
{
    public class PlotsRoomViewModel
    {
        public int PlotId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProjectName { get; set; }
        public decimal Amount { get; set; }
        public IEnumerable<RoomMappingList> RoomMappingLists { get; set; }
    }
}
