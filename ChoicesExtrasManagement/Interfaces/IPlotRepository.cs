using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.Interfaces
{
    public interface IPlotRepository
    {
        Plot GetPlot(int plotId);
        Task<IEnumerable<Plot>> GetAllPlotsByUser(string userId);
        Task<IEnumerable<Plot>> GetAllPlotsByProject(int projectId);
        Task<IEnumerable<Plot>> GetAllPlotsByPlotType(int plotTypeId);
        bool Add(Plot plot);
        bool Update(Plot plot);
        bool Delete(Plot plot);
        bool Save();
    }
}
