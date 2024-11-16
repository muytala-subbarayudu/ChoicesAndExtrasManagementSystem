using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.ViewModels
{
    public class PlotRoomChoiceExtraVM
    {
        public PlotTypeRoomMapping PlotTypeRoomMapping { get; set; }
        public List<CatalogueMapped> Choices { get; set; }
        public List<CatalogueMappedDual> Extras { get; set; }
    }
}
