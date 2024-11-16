namespace ChoicesExtrasManagement.ViewModels
{
    public class DashboardVM
    {
        public int ProjectsCount { get; set; }
        public int PlotsCount { get; set; }
        public int BuyersCount { get; set; }
        public int CatalogueCount { get; set; }
        public int UnSoldCount { get; set; }
        public int SoldCount { get; set; }
        public List<LocationPlotsVM> LocationPlotsVM { get; set; }
    }
}
