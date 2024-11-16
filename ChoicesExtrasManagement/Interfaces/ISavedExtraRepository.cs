using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.Interfaces
{
    public interface ISavedExtraRepository
    {
        Task<IEnumerable<SavedExtra>> GetSavedExtrasByPlotRoom(int? plotId, int? plotRoomId);
        Task<IEnumerable<SavedExtra>> GetSavedExtrasByPlot(int? plotId);
        Task<IEnumerable<SavedExtra>> GetSavedExtrasAsOutstanding(int? plotId);
        Task<IEnumerable<SavedExtra>> GetSavedExtrasAsOutstandingRoom(int? plotId, int plotRoomId);
        Task<IEnumerable<SavedExtra>> GetSavedExtrasAsOrders();
   
        bool Add(SavedExtra savedExtra);
        bool Update(SavedExtra savedExtra);
        bool Delete(SavedExtra savedExtra);
        bool Save();
    }
}
