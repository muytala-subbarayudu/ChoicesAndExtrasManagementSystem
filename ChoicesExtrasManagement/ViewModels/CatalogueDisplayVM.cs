using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.ViewModels
{
    public class CatalogueDisplayVM
    {
        public Plot Plot { get; set; }
        public PlotTypeRoomMapping PlotTypeRoomMapping { get; set; }
        public List<CatalogueMapped> CatalogueMappedList { get; set; }
        public string DisplayRoomName { get; set; }
    }
}
