using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.Interfaces
{
    public interface ISavedChoiceRepository
    {
        Task<IEnumerable<SavedChoice>> GetSavedChoicesByPlotRoom(int? plotId, int? plotRoomId);
        bool Add(SavedChoice savedChoice);
        bool Update(SavedChoice savedChoice);
        bool Delete(SavedChoice savedChoice);
        bool Save();
    }
}
