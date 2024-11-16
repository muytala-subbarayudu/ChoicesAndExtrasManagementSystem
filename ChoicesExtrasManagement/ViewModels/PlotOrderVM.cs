
using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.ViewModels
{
    public class PlotOrderVM
    {
        public Plot Plot { get; set; }
        public IEnumerable<SavedExtra> SavedExtras { get; set; }
    }
}
