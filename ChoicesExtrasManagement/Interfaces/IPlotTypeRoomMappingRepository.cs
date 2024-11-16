using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.Interfaces
{
    public interface IPlotTypeRoomMappingRepository
    {
        Task<IEnumerable<PlotTypeRoomMapping>> GetRoomMappingsByPlotType(int? plotTypeId);
        Task<PlotTypeRoomMapping> GetRoomPlotTypeById(int? roomPappingId);
        bool Add(PlotTypeRoomMapping plotTypeRoomMapping);
        bool Update(PlotTypeRoomMapping plotTypeRoomMapping);
        bool Delete(PlotTypeRoomMapping plotTypeRoomMapping);
        bool Save();
    }
}
