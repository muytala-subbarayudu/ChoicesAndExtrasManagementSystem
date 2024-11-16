using ChoicesExtrasManagement.Data;
using ChoicesExtrasManagement.Interfaces;
using ChoicesExtrasManagement.Models;
using ChoicesExtrasManagement.Repository;
using ChoicesExtrasManagement.Services;
using ChoicesExtrasManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using System.Data;
using System.Linq.Expressions;
using System.Transactions;

namespace ChoicesExtrasManagement.Controllers
{
    public class PlotController : Controller
    {
        public readonly IPlotRepository _plotRepository;
        private readonly UserManager<AppUser> _userManager;
        public readonly IPlotTypeRepository _plotTypeRepository;
        public readonly IRoomRepository _roomRepository;
        public readonly IPlotTypeRoomMappingRepository _plotTypeRoomMappingRepository;
        public readonly ICatalogueRepository _catalogueRepository;
        public readonly ISubCatalogueRepository _subCatalogueRepository;
        public readonly ICatalogueRoomPlotTypeMappingRepository _catalogueRoomPlotTypeMappingRepository;
        public readonly ISavedChoiceRepository _savedChoiceRepository;
        public readonly ISavedExtraRepository _savedExtraRepository;
        private readonly ChoicesExtrasManagementDbContext _db;
        public readonly IEmailService _emailService;


        public PlotController( 
            IPlotRepository plotRepository, 
            UserManager<AppUser> userManager, 
            IPlotTypeRepository plotTypeRepository,
            IRoomRepository roomRepository,
            IPlotTypeRoomMappingRepository plotTypeRoomMappingRepository,
            ICatalogueRepository catalogueRepository,
            ISubCatalogueRepository subCatalogueRepository,
            ICatalogueRoomPlotTypeMappingRepository catalogueRoomPlotTypeMappingRepository,
            ISavedChoiceRepository savedChoiceRepository,
            ISavedExtraRepository savedExtraRepository,
            ChoicesExtrasManagementDbContext context,
            IEmailService emailService
            )
        {
            _plotRepository = plotRepository;
            _userManager = userManager;
            _plotTypeRepository = plotTypeRepository;
            _roomRepository = roomRepository;
            _plotTypeRoomMappingRepository = plotTypeRoomMappingRepository;
            _catalogueRepository = catalogueRepository;
            _subCatalogueRepository = subCatalogueRepository;
            _catalogueRoomPlotTypeMappingRepository = catalogueRoomPlotTypeMappingRepository;
            _savedChoiceRepository = savedChoiceRepository;
            _savedExtraRepository = savedExtraRepository;
            _db = context;
            _emailService = emailService;
        }
        public async Task<IActionResult> Index()
        {
            string userId = _userManager.GetUserAsync(HttpContext.User).Result.Id;

            IEnumerable<Plot> userPlots = await _plotRepository.GetAllPlotsByUser(userId);

            return View(userPlots);
        }

        public async Task<IActionResult> Detail(int id)
        {
            PlotsRoomViewModel pvm = new PlotsRoomViewModel();

            Plot plot = _plotRepository.GetPlot(id);
            IEnumerable<PlotTypeRoomMapping> roomMappings = await _plotTypeRoomMappingRepository.GetRoomMappingsByPlotType(plot.PlotTypeId);

            pvm.PlotId = plot.Id;
            pvm.Name = plot.PlotType.Name;
            pvm.Description = plot.PlotType.Description;
            pvm.Amount = plot.PlotType.Amount;
            pvm.ProjectName = plot.Project.Name;

            List<RoomMappingList> RoomMappingLists = new List<RoomMappingList>();

            var drooms = roomMappings.DistinctBy(x => x.RoomId);
            foreach (var item in drooms)
            {
                RoomMappingList roomMappingList = new RoomMappingList();

                roomMappingList.room = item.Room;
                roomMappingList.count = roomMappings.Where(x => x.RoomId == item.RoomId && x.PlotTypeId == plot.PlotTypeId).Count();

                RoomMappingLists.Add(roomMappingList);
            }

            pvm.RoomMappingLists = RoomMappingLists;

            return View(pvm);
        }

        [Authorize(Roles = "home-buyer")]
        public async Task<IActionResult> ManageMyPlot(int id)
        {

            return View();
        }

        [Authorize(Roles = "home-buyer")]
        public async Task<IActionResult> PlotOverview(int id)
        {
            string userId = _userManager.GetUserAsync(HttpContext.User).Result.Id;

            IEnumerable<Plot> userPlots = await _plotRepository.GetAllPlotsByUser(userId);

            Plot plot = userPlots.FirstOrDefault(p=>p.Id == id);

            return View(plot);
        }

        [Authorize(Roles = "home-buyer")]
        public async Task<IActionResult> MyChoicesExtras(int id)
        {
            string userId = _userManager.GetUserAsync(HttpContext.User).Result.Id;

            IEnumerable<Plot> userPlots = await _plotRepository.GetAllPlotsByUser(userId);

            PlotRoomVM plotRoomVM = new PlotRoomVM();

            Plot plot = userPlots.FirstOrDefault(p => p.Id == id);

            plotRoomVM.Plot = plot;

            var roomsMapped = await _plotTypeRoomMappingRepository.GetRoomMappingsByPlotType(plot.PlotTypeId);

            var groupedRooms = roomsMapped.OrderBy(x=>x.RoomId).GroupBy(r=>r.RoomId).ToList();

            List<RoomVM> roomVMs = new List<RoomVM>();

            foreach (var item in groupedRooms)
            {
                var subList = item.ToList();
                if (subList.Count() == 1)
                {
                    RoomVM roomVM = new RoomVM();
                    roomVM.Id = subList.FirstOrDefault().Id;
                    roomVM.Name = subList.FirstOrDefault().Room.Name;
                    roomVM.Description = subList.FirstOrDefault().Room.Description;
                    roomVMs.Add(roomVM);
                }
                else if (subList.Count() > 1)
                {
                    int count = 1;
                    foreach (var sub in subList)
                    {
                        RoomVM roomVM = new RoomVM();
                        roomVM.Id = sub.Id;
                        roomVM.Name = sub.Room.Name + " " + count.ToString();
                        roomVM.Description = sub.Room.Description;
                        roomVMs.Add(roomVM);
                        count++;
                    }
                }
            }
            plotRoomVM.RoomVMs = roomVMs;

            return View(plotRoomVM);
        }

        [Authorize(Roles = "home-buyer")]
        public async Task<IActionResult> PlotOrders(int id)
        {
            PlotOrderVM plotOrderVM = new PlotOrderVM();

            string userId = _userManager.GetUserAsync(HttpContext.User).Result.Id;

            IEnumerable<Plot> userPlots = await _plotRepository.GetAllPlotsByUser(userId);

            PlotRoomVM plotRoomVM = new PlotRoomVM();

            Plot plot = userPlots.FirstOrDefault(p => p.Id == id);

            var savedExtras = await _savedExtraRepository.GetSavedExtrasByPlot(id);

            savedExtras = savedExtras.Where(x=>x.TransactionId != null).OrderByDescending(y=>y.Transaction.Date).ToList();

            savedExtras = savedExtras.DistinctBy(x => x.TransactionId).ToList();

            plotOrderVM.Plot = plot;
            plotOrderVM.SavedExtras = savedExtras;

            return View(plotOrderVM);
        }

        [Authorize(Roles = "home-buyer")]
        public async Task<IActionResult> PlotNotifications(int id)
        {
            NotificationVM notificationVM = new NotificationVM();
            var plot = _db.Plots
                .Include(pr => pr.Project)
                .Include(pr => pr.PlotType)
                .Include(l=>l.Project.Location)
                .Where(x => x.Id == id).FirstOrDefault();
            var notifications = await _db.Notifications
                .Include(p=>p.Plot).Where(x => x.PlotId == id).ToListAsync();

            notificationVM.Plot = plot;
            notificationVM.NotificationsList = notifications.OrderByDescending(x=>x.MessageDate).ToList();

            return View(notificationVM);
        }

        [Authorize]
        public async Task<IActionResult> MyInvoice(int plotid, string transactionid)
        {
            string userId = _userManager.GetUserAsync(HttpContext.User).Result.Id;

            IEnumerable<Plot> userPlots = await _plotRepository.GetAllPlotsByUser(userId);

            PlotRoomVM plotRoomVM = new PlotRoomVM();

            Plot plot = userPlots.FirstOrDefault(p => p.Id == plotid);

            var savedExtras = await _savedExtraRepository.GetSavedExtrasByPlot(plotid);

            savedExtras = savedExtras.Where(x => x.TransactionId == transactionid).ToList();

            return View(savedExtras);
        }

        public async Task<IActionResult> MyChoices(int plotid, int mappedroomid)
        {
            CatalogueDisplayVM catalogueDisplayVM = new CatalogueDisplayVM();

            string userId = _userManager.GetUserAsync(HttpContext.User).Result.Id;

            IEnumerable<Plot> userPlots = await _plotRepository.GetAllPlotsByUser(userId);

            Plot plot = userPlots.FirstOrDefault(p => p.Id == plotid);

            var roomMapping = await _plotTypeRoomMappingRepository.GetRoomPlotTypeById(mappedroomid);

            int? roomid = roomMapping.RoomId;

            Room room = await _roomRepository.GetRoomById(roomid);

            var allCataloguesMapped = await _catalogueRoomPlotTypeMappingRepository.GetCatalogueMappingsToDisplayByPlotTyeAndRoom(plot.PlotTypeId, roomid);

            List<CatalogueMapped> cataloguesMappedList = new List<CatalogueMapped>();

            var savedChoicesList = await _savedChoiceRepository.GetSavedChoicesByPlotRoom(plotid, mappedroomid);

            foreach (var item in allCataloguesMapped)
            {
                
                if (item.ChoiceorExtra == false)
                {
                    CatalogueMapped catalogueMapped = new CatalogueMapped();

                    catalogueMapped.Catalogue = await _catalogueRepository.GetCatalogueItem(item.CatalogueId.Value);
                    catalogueMapped.SubCatalogues = await _subCatalogueRepository.GetAllSubCatalogueItems(item.CatalogueId.Value);
                    catalogueMapped.SubCatalogueMapped = savedChoicesList.FirstOrDefault(c=>c.CatalogueId == item.CatalogueId)?.SubCatalogueId;
                    catalogueMapped.SubCatalogueItemCount = 1;
                    catalogueMapped.isDisplayed = true;

                    cataloguesMappedList.Add(catalogueMapped);
                }
            }

            catalogueDisplayVM.Plot = plot;
            catalogueDisplayVM.PlotTypeRoomMapping = roomMapping;
            catalogueDisplayVM.DisplayRoomName = roomMapping.Room.Name;
            catalogueDisplayVM.CatalogueMappedList = cataloguesMappedList;


            return View(catalogueDisplayVM);
        }

        public async Task<IActionResult> MyExtras(int plotid, int mappedroomid)
        {
            ExtrasDisplayVM extrasDisplayVM = new ExtrasDisplayVM();

            string userId = _userManager.GetUserAsync(HttpContext.User).Result.Id;

            IEnumerable<Plot> userPlots = await _plotRepository.GetAllPlotsByUser(userId);

            Plot plot = userPlots.FirstOrDefault(p => p.Id == plotid);

            var roomMapping = await _plotTypeRoomMappingRepository.GetRoomPlotTypeById(mappedroomid);

            int? roomid = roomMapping.RoomId;

            Room room = await _roomRepository.GetRoomById(roomid);

            var allCataloguesMapped = await _catalogueRoomPlotTypeMappingRepository.GetCatalogueMappingsToDisplayByPlotTyeAndRoom(plot.PlotTypeId, roomid);

            List<CatalogueMappedDual> extrasPurchasedList = new List<CatalogueMappedDual>();
            List<CatalogueMappedDual> extrasToBePurchasedList = new List<CatalogueMappedDual>();
            List<CatalogueMapped> allExtrasMappedList = new List<CatalogueMapped>();

            var savedExtrasList = await _savedExtraRepository.GetSavedExtrasByPlotRoom(plotid, mappedroomid);

            foreach (var item in allCataloguesMapped)
            {
                if (item.ChoiceorExtra == true)
                {
                    var currentExtra = savedExtrasList.FirstOrDefault(c => c.CatalogueId == item.CatalogueId && c.Catalogue.SingleOrMultiple == false);

                    CatalogueMapped catalogueMapped = new CatalogueMapped();

                    catalogueMapped.Catalogue = await _catalogueRepository.GetCatalogueItem(item.CatalogueId.Value);
                    catalogueMapped.SubCatalogues = await _subCatalogueRepository.GetAllSubCatalogueItems(item.CatalogueId.Value);
                    //catalogueMapped.SubCatalogueMapped = currentExtra?.SubCatalogueId;
                    //catalogueMapped.SubCatalogueItemCount = currentExtra?.Quantity;
                    catalogueMapped.isDisplayed = (currentExtra == null)? false : true; // based on single or multiple

                    allExtrasMappedList.Add(catalogueMapped);
                }

                if (item.ChoiceorExtra == false && item.Catalogue.SingleOrMultiple == true)
                {
                    var currentExtra = savedExtrasList.FirstOrDefault(c => c.CatalogueId == item.CatalogueId && c.Catalogue.SingleOrMultiple == false);

                    CatalogueMapped catalogueMapped = new CatalogueMapped();

                    catalogueMapped.Catalogue = await _catalogueRepository.GetCatalogueItem(item.CatalogueId.Value);
                    catalogueMapped.SubCatalogues = await _subCatalogueRepository.GetAllSubCatalogueItems(item.CatalogueId.Value);
                    //catalogueMapped.SubCatalogueMapped = currentExtra?.SubCatalogueId;
                    //catalogueMapped.SubCatalogueItemCount = currentExtra?.Quantity;
                    catalogueMapped.isDisplayed = (currentExtra == null) ? false : true; // based on single or multiple
                   

                    allExtrasMappedList.Add(catalogueMapped);
                }
            }

            foreach (var item in savedExtrasList)
            {
                CatalogueMappedDual catalogueMapped = new CatalogueMappedDual();

                catalogueMapped.Catalogue = await _catalogueRepository.GetCatalogueItem(item.CatalogueId);
                catalogueMapped.SubCatalogues = await _subCatalogueRepository.GetAllSubCatalogueItems(item.CatalogueId);
                catalogueMapped.SubCatalogueMapped = item?.SubCatalogueId;
                catalogueMapped.SubCatalogueItemCount = item?.Quantity;
                catalogueMapped.isDisplayed = false; // based on single or multiple
                catalogueMapped.PricePaid = item.AmountPaid;

                // allExtrasMappedList.Add(catalogueMapped);

                if (item?.TransactionId == null)
                {
                    extrasToBePurchasedList.Add(catalogueMapped);
                }
                else
                {
                    extrasPurchasedList.Add(catalogueMapped);
                }

                extrasPurchasedList = extrasPurchasedList
                    .GroupBy(x=>x.SubCatalogueMapped)
                    .Select(e=> new CatalogueMappedDual
                    {
                        Catalogue = e.FirstOrDefault().Catalogue,
                        SubCatalogues = e.FirstOrDefault().SubCatalogues,
                        SubCatalogueMapped = e.FirstOrDefault().SubCatalogueMapped,
                        SubCatalogueItemCount = e.Sum(c=>c.SubCatalogueItemCount).Value,
                        isDisplayed = false, // based on single or multiple
                        PricePaid = e.Sum(c=>c.PricePaid).Value,

                    }
                    ).ToList();
            }

            extrasDisplayVM.Plot = plot;
            extrasDisplayVM.PlotTypeRoomMapping = roomMapping;
            extrasDisplayVM.DisplayRoomName = roomMapping.Room.Name;
            extrasDisplayVM.AllExtrasMappedList = allExtrasMappedList;
            extrasDisplayVM.ExtrasPurchasedList = extrasPurchasedList;
            extrasDisplayVM.ExtrasToBePurchasedList = extrasToBePurchasedList;

            return View(extrasDisplayVM);
        }

        public async Task<bool> SaveChoices(string jsonResult, int plotRoomId, int plotId)
        {
            var resultList = JsonConvert.DeserializeObject<List<CatalogueMapped>>(jsonResult);
            var plotRoomMapping = await _plotTypeRoomMappingRepository.GetRoomPlotTypeById(plotRoomId);
            var savedChoicesList = await _savedChoiceRepository.GetSavedChoicesByPlotRoom(plotId, plotRoomId);
            try
            {
                foreach (var item in resultList)
                {
                    var exists = savedChoicesList.Any(x => x.CatalogueId == item.Catalogue.Id);

                    if (exists)
                    {
                        var existingRecord = savedChoicesList.FirstOrDefault(x => x.CatalogueId.Equals(item.Catalogue.Id));

                        existingRecord.SubCatalogueId = item.SubCatalogueMapped;

                        _savedChoiceRepository.Update(existingRecord);
                    }
                    else
                    {
                        SavedChoice savedChoice = new SavedChoice();
                        savedChoice.PlotId = plotId;
                        savedChoice.PlotRoomId = plotRoomId;
                        savedChoice.CatalogueId = item.Catalogue.Id;
                        savedChoice.SubCatalogueId = item.SubCatalogueMapped;
                        savedChoice.isSubmitted = false;

                        _savedChoiceRepository.Add(savedChoice);
                    }
                }
            }
            catch (Exception ex) { return false; }
            return true;
        }

        public async Task<bool> SaveExtras(string jsonResult, int plotRoomId, int plotId)
        {
            var resultList = JsonConvert.DeserializeObject<List<CatalogueMapped>>(jsonResult);

            var plotRoomMapping = await _plotTypeRoomMappingRepository.GetRoomPlotTypeById(plotRoomId);
            var savedExtrasList = await _savedExtraRepository.GetSavedExtrasAsOutstandingRoom(plotId,plotRoomMapping.Id);


            try
            {

                //delete all records unpaid
                foreach (var item in savedExtrasList)
                {
                    _savedExtraRepository.Delete(item);
                }

                // add all records again as new list
                foreach (var item in resultList)
                {
                        var currentPrice = item.SubCatalogues.FirstOrDefault(s => s.Id == item.SubCatalogueMapped).Price;

                        SavedExtra saved = new SavedExtra();
                        saved.PlotId = plotId;
                        saved.PlotRoomId = plotRoomId;
                        saved.CatalogueId = item.Catalogue.Id;
                        saved.SubCatalogueId = item.SubCatalogueMapped.Value;
                        saved.Quantity = item.SubCatalogueItemCount.Value;
                        saved.PricePer = currentPrice;
                        saved.AmountPaid = currentPrice * item.SubCatalogueItemCount.Value;
                        saved.isSubmitted = false;

                        _savedExtraRepository.Add(saved);
                }

            }
            catch (Exception ex) { return false; }
            return true;
        }

        public async Task<JsonResult> GetAllUserChoices(int plotid, int mappedroomid, string userId = null)
        {
            var savedChoicesList = await _savedChoiceRepository.GetSavedChoicesByPlotRoom(plotid, mappedroomid);


            CatalogueDisplayVM catalogueDisplayVM = new CatalogueDisplayVM();

            if(userId == null ) userId = _userManager.GetUserAsync(HttpContext.User).Result.Id;

            IEnumerable<Plot> userPlots = await _plotRepository.GetAllPlotsByUser(userId);

            Plot plot = userPlots.FirstOrDefault(p => p.Id == plotid);

            var roomMapping = await _plotTypeRoomMappingRepository.GetRoomPlotTypeById(mappedroomid);

            int? roomid = roomMapping.RoomId;



            //Room room = await _roomRepository.GetRoomById(roomid);

            var allCataloguesMapped = await _catalogueRoomPlotTypeMappingRepository.GetCatalogueMappingsToDisplayByPlotTyeAndRoom(plot.PlotTypeId, roomid);


            List<CatalogueMapped> cataloguesMappedList = new List<CatalogueMapped>();

            foreach (var item in allCataloguesMapped)
            {

                if (item.ChoiceorExtra == false)
                {
                    CatalogueMapped catalogueMapped = new CatalogueMapped();

                    catalogueMapped.Catalogue = await _catalogueRepository.GetCatalogueItem(item.CatalogueId.Value);
                    catalogueMapped.SubCatalogues = await _subCatalogueRepository.GetAllSubCatalogueItems(item.CatalogueId.Value);
                    catalogueMapped.SubCatalogueMapped = savedChoicesList.FirstOrDefault(c => c.CatalogueId == item.CatalogueId)?.SubCatalogueId;
                    catalogueMapped.SubCatalogueItemCount = 1;
                    catalogueMapped.isDisplayed = true;

                    cataloguesMappedList.Add(catalogueMapped);
                }
            }

            catalogueDisplayVM.Plot = plot;
            catalogueDisplayVM.PlotTypeRoomMapping = roomMapping;
            catalogueDisplayVM.DisplayRoomName = roomMapping.Room.Name;
            catalogueDisplayVM.CatalogueMappedList = cataloguesMappedList;


            return Json(catalogueDisplayVM);
        }

        public async Task<JsonResult> GetAllUserExtras(int plotid, int mappedroomid, string userId = null)
        {
            ExtrasDisplayVM extrasDisplayVM = new ExtrasDisplayVM();

            if (userId == null) userId = _userManager.GetUserAsync(HttpContext.User).Result.Id;

            IEnumerable<Plot> userPlots = await _plotRepository.GetAllPlotsByUser(userId);

            Plot plot = userPlots.FirstOrDefault(p => p.Id == plotid);

            var roomMapping = await _plotTypeRoomMappingRepository.GetRoomPlotTypeById(mappedroomid);

            int? roomid = roomMapping.RoomId;

            Room room = await _roomRepository.GetRoomById(roomid);

            var allCataloguesMapped = await _catalogueRoomPlotTypeMappingRepository.GetCatalogueMappingsToDisplayByPlotTyeAndRoom(plot.PlotTypeId, roomid);

            List<CatalogueMappedDual> extrasPurchasedList = new List<CatalogueMappedDual>();
            List<CatalogueMappedDual> extrasToBePurchasedList = new List<CatalogueMappedDual>();
            List<CatalogueMapped> allExtrasMappedList = new List<CatalogueMapped>();

            var savedExtrasList = await _savedExtraRepository.GetSavedExtrasByPlotRoom(plotid, mappedroomid);

            foreach (var item in allCataloguesMapped)
            {
                if (item.ChoiceorExtra == true)
                {
                    var currentExtra = savedExtrasList.FirstOrDefault(c => c.CatalogueId == item.CatalogueId && c.Catalogue.SingleOrMultiple == false);

                    CatalogueMapped catalogueMapped = new CatalogueMapped();

                    catalogueMapped.Catalogue = await _catalogueRepository.GetCatalogueItem(item.CatalogueId.Value);
                    catalogueMapped.SubCatalogues = await _subCatalogueRepository.GetAllSubCatalogueItems(item.CatalogueId.Value);
                    //catalogueMapped.SubCatalogueMapped = currentExtra?.SubCatalogueId;
                    //catalogueMapped.SubCatalogueItemCount = currentExtra?.Quantity;
                    catalogueMapped.isDisplayed = (currentExtra == null) ? false : true; // based on single or multiple

                    allExtrasMappedList.Add(catalogueMapped);
                }
            }

            foreach (var item in savedExtrasList)
            {
                CatalogueMappedDual catalogueMapped = new CatalogueMappedDual();

                catalogueMapped.Catalogue = await _catalogueRepository.GetCatalogueItem(item.CatalogueId);
                catalogueMapped.SubCatalogues = await _subCatalogueRepository.GetAllSubCatalogueItems(item.CatalogueId);
                catalogueMapped.SubCatalogueMapped = item?.SubCatalogueId;
                catalogueMapped.SubCatalogueItemCount = item?.Quantity;
                catalogueMapped.isDisplayed = false; // based on single or multiple
                catalogueMapped.PricePaid = item.AmountPaid;

                // allExtrasMappedList.Add(catalogueMapped);

                if (item?.TransactionId == null)
                {
                    extrasToBePurchasedList.Add(catalogueMapped);
                }
                else
                {
                    extrasPurchasedList.Add(catalogueMapped);
                }
            }

            extrasPurchasedList = extrasPurchasedList
                    .GroupBy(x => x.SubCatalogueMapped)
                    .Select(e => new CatalogueMappedDual
                    {
                        Catalogue = e.FirstOrDefault().Catalogue,
                        SubCatalogues = e.FirstOrDefault().SubCatalogues,
                        SubCatalogueMapped = e.FirstOrDefault().SubCatalogueMapped,
                        SubCatalogueItemCount = e.Sum(c => c.SubCatalogueItemCount).Value,
                        isDisplayed = false, // based on single or multiple
                        PricePaid = e.Sum(c=>c.PricePaid).Value
                    }
                    ).ToList();

            extrasDisplayVM.Plot = plot;
            extrasDisplayVM.PlotTypeRoomMapping = roomMapping;
            extrasDisplayVM.DisplayRoomName = roomMapping.Room.Name;
            extrasDisplayVM.AllExtrasMappedList = allExtrasMappedList;
            extrasDisplayVM.ExtrasPurchasedList = extrasPurchasedList;
            extrasDisplayVM.ExtrasToBePurchasedList = extrasToBePurchasedList;

            return Json(extrasDisplayVM);
        }

        public async Task<JsonResult> GetAllUserOutstanding(int plotid,  string userId = null)
        {
            IEnumerable<SavedExtra> savedOrders = new List<SavedExtra>();

            savedOrders = await _savedExtraRepository.GetSavedExtrasAsOutstanding(plotid);


            decimal totalToPay = 0;

            foreach (var item in savedOrders)
            {
                totalToPay += item.Quantity * item.SubCatalogue.Price;
            }

            TempData["toPay"] = totalToPay.ToString();

            return Json(savedOrders);
        }

        public async Task<JsonResult> GetAllUserOrders(int? plotid =null)
        {
            List<TransactionsVM> transactionsVMs = new List<TransactionsVM>();

            IEnumerable<SavedExtra> savedOrders = new List<SavedExtra>();

            if (plotid != null)
            {
                savedOrders = await _savedExtraRepository.GetSavedExtrasAsOrders();
            }
            else
            {
                savedOrders = await _savedExtraRepository.GetSavedExtrasByPlot(plotid);

            }

            savedOrders = savedOrders.GroupBy(x => x.TransactionId).FirstOrDefault().ToList();

            foreach (var item in savedOrders)
            {
                TransactionsVM transactionsVM = new TransactionsVM();

                transactionsVM.Plot = item.Plot;
                transactionsVM.PaymentTransaction = item.Transaction;

                transactionsVMs.Add(transactionsVM);
            }


            return Json(transactionsVMs);
        }

        public async Task<bool> SendFeedbackEmail(int plotid, string message)
        {
            try{
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                string userId = _userManager.GetUserAsync(HttpContext.User).Result.Id;

                IEnumerable<Plot> userPlots = await _plotRepository.GetAllPlotsByUser(userId);

                Plot plot = userPlots.FirstOrDefault(p => p.Id == plotid);

                string messageString = "\n Project: " + plot.Project.Name ;
                messageString += "\n\n Plot: PL" + plot.Id + " - " + plot.PlotType.Name;
                messageString += "\n\n Message: " + message;

                _emailService.SendEmailSelfAsync("Feedback/Suggesstion", messageString);
                return true;
            }
            catch { 
                return false;
            }
        }

        public async Task<IActionResult> ChoicesExtras(int plotid)
        {

            Plot plot = _plotRepository.GetPlot(plotid);

            List<PlotRoomChoiceExtraVM> plotRoomChoiceExtraVMList = new List<PlotRoomChoiceExtraVM>();

            IEnumerable<PlotTypeRoomMapping> roomMappings = await _plotTypeRoomMappingRepository.GetRoomMappingsByPlotType(plot.PlotTypeId);

            foreach (var roomMapping in roomMappings)
            {
                PlotRoomChoiceExtraVM plotRoomChoiceExtraVM = new PlotRoomChoiceExtraVM();

                //Choices
                var allCataloguesMapped = await _catalogueRoomPlotTypeMappingRepository.GetCatalogueMappingsToDisplayByPlotTyeAndRoom(plot.PlotTypeId, roomMapping.Room.Id);
                var savedChoicesList = await _savedChoiceRepository.GetSavedChoicesByPlotRoom(plotid, roomMapping.Id);

                List<CatalogueMapped> cataloguesMappedList = new List<CatalogueMapped>();

                foreach (var item in allCataloguesMapped)
                {

                    if (item.ChoiceorExtra == false)
                    {
                        CatalogueMapped catalogueMapped = new CatalogueMapped();

                        catalogueMapped.Catalogue = await _catalogueRepository.GetCatalogueItem(item.CatalogueId.Value);
                        catalogueMapped.SubCatalogues = await _subCatalogueRepository.GetAllSubCatalogueItems(item.CatalogueId.Value);
                        catalogueMapped.SubCatalogueMapped = savedChoicesList.FirstOrDefault(c => c.CatalogueId == item.CatalogueId)?.SubCatalogueId;
                        catalogueMapped.SubCatalogueItemCount = 1;
                        catalogueMapped.isDisplayed = true;

                        cataloguesMappedList.Add(catalogueMapped);
                    }
                }

                //Extra
                List<CatalogueMappedDual> allExtrasToDisplay = new List<CatalogueMappedDual>();

                var savedExtrasList = await _savedExtraRepository.GetSavedExtrasByPlotRoom(plotid, roomMapping.Id);

                var paidList = savedExtrasList.Where(x => x.TransactionId != null).ToList();
                var unPaidList = savedExtrasList.Where(x => x.TransactionId == null).ToList();
                foreach (var item in paidList)
                {
                    CatalogueMappedDual catalogueMapped = new CatalogueMappedDual();

                    catalogueMapped.Catalogue = await _catalogueRepository.GetCatalogueItem(item.CatalogueId);
                    catalogueMapped.SubCatalogues = await _subCatalogueRepository.GetAllSubCatalogueItems(item.CatalogueId);
                    catalogueMapped.SubCatalogueMapped = item?.SubCatalogueId;
                    catalogueMapped.SubCatalogueItemCount = item?.Quantity;
                    catalogueMapped.isDisplayed = false; // based on single or multiple
                    catalogueMapped.Transaction = item?.TransactionId;

                    if (item?.TransactionId != null)
                    {
                        catalogueMapped.PricePaid = item.AmountPaid;
                        catalogueMapped.YesOrNo = true;

                    }
                    else
                    {
                        catalogueMapped.PricePaid = null;
                        catalogueMapped.YesOrNo = false;
                    }


                    allExtrasToDisplay.Add(catalogueMapped);
                }

                allExtrasToDisplay = allExtrasToDisplay
                    .GroupBy(x => x.SubCatalogueMapped)
                    .Select(e => new CatalogueMappedDual
                    {
                        Catalogue = e.FirstOrDefault().Catalogue,
                        SubCatalogues = e.FirstOrDefault().SubCatalogues,
                        SubCatalogueMapped = e.FirstOrDefault().SubCatalogueMapped,
                        SubCatalogueItemCount = e.Sum(c => c.SubCatalogueItemCount).Value,
                        isDisplayed = false, // based on single or multiple
                        Transaction = e.FirstOrDefault()?.Transaction,
                        PricePaid = e.Sum(x => x.PricePaid),
                        YesOrNo = e.FirstOrDefault().YesOrNo
                    }
                    ).ToList();

                foreach (var item in unPaidList)
                {
                    CatalogueMappedDual catalogueMapped = new CatalogueMappedDual();

                    catalogueMapped.Catalogue = await _catalogueRepository.GetCatalogueItem(item.CatalogueId);
                    catalogueMapped.SubCatalogues = await _subCatalogueRepository.GetAllSubCatalogueItems(item.CatalogueId);
                    catalogueMapped.SubCatalogueMapped = item?.SubCatalogueId;
                    catalogueMapped.SubCatalogueItemCount = item?.Quantity;
                    catalogueMapped.isDisplayed = false; // based on single or multiple
                    catalogueMapped.Transaction = item?.TransactionId;

                    if (item?.TransactionId != null)
                    {
                        catalogueMapped.PricePaid = item.AmountPaid;
                        catalogueMapped.YesOrNo = true;

                    }
                    else
                    {
                        catalogueMapped.PricePaid = null;
                        catalogueMapped.YesOrNo = false;
                    }


                    allExtrasToDisplay.Add(catalogueMapped);
                }

                plotRoomChoiceExtraVM.PlotTypeRoomMapping = roomMapping;
                plotRoomChoiceExtraVM.Choices = cataloguesMappedList;
                plotRoomChoiceExtraVM.Extras = allExtrasToDisplay;

                plotRoomChoiceExtraVMList.Add(plotRoomChoiceExtraVM);

            }


            return View(plotRoomChoiceExtraVMList);
        }

    }
}
