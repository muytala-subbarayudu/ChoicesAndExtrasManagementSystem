using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.Interfaces
{
    public interface ICatalogueRoomPlotTypeMappingRepository
    {
        Task<IEnumerable<CatalogueRoomPlotTypeMapping>> GetCatalogueRoomPlotTypeMappings(int catalogueItemId);
        Task<IEnumerable<CatalogueRoomPlotTypeMapping>> GetAllCatalogueMappingsByPlotTye(int plotTypeId);
        Task<IEnumerable<CatalogueRoomPlotTypeMapping>> GetAllCatalogueMappingsByPlotTyeAndRoom(int? plotTypeId, int? roomId);
        Task<IEnumerable<CatalogueRoomPlotTypeMapping>> GetCatalogueMappingsToDisplayByPlotTyeAndRoom(int? plotTypeId, int? roomId);   
        bool Add(CatalogueRoomPlotTypeMapping catalogueRoomPlotTypeMapping);
        bool Update(CatalogueRoomPlotTypeMapping catalogueRoomPlotTypeMapping);
        bool Delete(CatalogueRoomPlotTypeMapping catalogueRoomPlotTypeMapping);
        bool Save();
    }
}
