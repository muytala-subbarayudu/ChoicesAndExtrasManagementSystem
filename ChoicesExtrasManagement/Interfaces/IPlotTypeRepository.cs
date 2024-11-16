using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.Interfaces
{
    public interface IPlotTypeRepository
    {
        Task<IEnumerable<PlotType>> GetAllPlotTypes();
        bool Add(PlotType plotType);
        bool Update(PlotType plotType);
        bool Delete(PlotType plotType);
        bool Save();
    }
}
