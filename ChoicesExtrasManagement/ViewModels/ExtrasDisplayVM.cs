using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.ViewModels
{
    public class ExtrasDisplayVM
    {
        public Plot Plot { get; set; }
        public PlotTypeRoomMapping PlotTypeRoomMapping { get; set; }
        public List<CatalogueMappedDual> ExtrasPurchasedList { get; set; }
        public List<CatalogueMappedDual> ExtrasToBePurchasedList { get; set; }
        public List<CatalogueMapped> AllExtrasMappedList { get; set; }
        public string DisplayRoomName { get; set; }
    }
}
