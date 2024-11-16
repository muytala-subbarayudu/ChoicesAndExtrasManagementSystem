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
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Twilio.Rest.Trunking.V1;
using Twilio.TwiML.Messaging;
using static System.Reflection.Metadata.BlobBuilder;

namespace ChoicesExtrasManagement.Controllers
{
    [Authorize(Roles = "home-developer")]
    public class AdminController : Controller
    {
        public readonly ILocationRepository _locationRepository;
        public readonly IPlotTypeRepository _plotTypeRepository;
        public readonly IRoomRepository _roomRepository;
        public readonly IProjectRepository _projectRepository;
        public readonly IPlotRepository _plotRepository;
        public readonly IPlotTypeRoomMappingRepository _plotTypeRoomMappingRepository;
        private readonly UserManager<AppUser> _userManager;
        public readonly ICatalogueRepository _catalogueRepository;
        public readonly ISubCatalogueRepository _subCatalogueRepository;
        public readonly ICatalogueRoomPlotTypeMappingRepository _catalogueRoomPlotTypeMappingRepository;
        private readonly ChoicesExtrasManagementDbContext _db;
        public readonly ISavedChoiceRepository _savedChoiceRepository;
        public readonly IEmailService _emailService;
        public readonly ISavedExtraRepository _savedExtraRepository;

        public AdminController
        (ILocationRepository locationRepository,
            IPlotTypeRepository plotTypeRepository,
            IRoomRepository roomRepository,
            IProjectRepository projectRepository,
            IPlotRepository plotRepository,
            UserManager<AppUser> userManager,
            IPlotTypeRoomMappingRepository plotTypeRoomMappingRepository,
            ICatalogueRepository catalogueRepository,
            ISubCatalogueRepository subCatalogueRepository,
            ICatalogueRoomPlotTypeMappingRepository catalogueRoomPlotTypeMappingRepository,
            ChoicesExtrasManagementDbContext context,
            ISavedChoiceRepository savedChoiceRepository,
            ISavedExtraRepository savedExtraRepository,
            IEmailService emailService
        )
        {
            _locationRepository = locationRepository;
            _plotTypeRepository = plotTypeRepository;
            _roomRepository = roomRepository;
            _projectRepository = projectRepository;
            _plotRepository = plotRepository;
            _userManager = userManager;
            _plotTypeRoomMappingRepository = plotTypeRoomMappingRepository;
            _catalogueRepository = catalogueRepository;
            _subCatalogueRepository = subCatalogueRepository;
            _catalogueRoomPlotTypeMappingRepository = catalogueRoomPlotTypeMappingRepository;
            _db = context;
            _savedChoiceRepository = savedChoiceRepository;
            _savedExtraRepository = savedExtraRepository;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ManageLocations()
        {
            IEnumerable<Location> locations = await _locationRepository.GetAllLocations();
            return View(locations);
        }

        public bool AddLocation(string location)
        {
            bool result = _locationRepository.Add(new Location() { Name = location });
            return result;
        }

        public async Task<IActionResult> ManagePlotTypes()
        {
            IEnumerable<PlotType> plotTypes = await _plotTypeRepository.GetAllPlotTypes();

            IEnumerable<PlotTypeRoomMapping> plotTypeRoomMappings;
            IEnumerable<Room> allRooms = await _roomRepository.GetAllRooms();


            List<PlotTypesViewModel> plotTypesViewModelList = new List<PlotTypesViewModel>();
            List<Room> mappedRooms = new List<Room>();

            foreach (PlotType plotType in plotTypes)
            {
                PlotTypesViewModel plotTypeViewModel = new PlotTypesViewModel();
                plotTypeRoomMappings = await _plotTypeRoomMappingRepository.GetRoomMappingsByPlotType(plotType.Id);

                plotTypeViewModel.Id = plotType.Id;
                plotTypeViewModel.Amount = plotType.Amount;
                plotTypeViewModel.Description = plotType.Description;
                plotTypeViewModel.Name = plotType.Name;
                plotTypeViewModel.AllRooms = allRooms;


                foreach (PlotTypeRoomMapping plotTypeRoomMapping in plotTypeRoomMappings)
                {
                    mappedRooms.Add(plotTypeRoomMapping.Room);
                }

                if (mappedRooms.Count > 0)
                {
                    plotTypeViewModel.RoomDetails = GetRoomDetailsBasedOnMappedRooms(mappedRooms);
                    plotTypeViewModel.Rooms = mappedRooms;
                }
                else
                {
                    plotTypeViewModel.RoomDetails = null;
                    plotTypeViewModel.Rooms = null;
                }

                plotTypesViewModelList.Add(plotTypeViewModel);
                mappedRooms.Clear();
            }

            plotTypesViewModelList = plotTypesViewModelList.OrderBy(a => a.Amount).ToList();

            return View(plotTypesViewModelList);
        }

        public bool AddPlotType(string plotType, string description, decimal amount)
        {
            bool result = _plotTypeRepository.Add(new PlotType() { Name = plotType, Description = description, Amount = amount });
            return result;
        }

        public async Task<IActionResult> ManageRooms()
        {
            IEnumerable<Room> plotTypes = await _roomRepository.GetAllRooms();
            return View(plotTypes);
        }

        public bool AddRoom(string name, string description)
        {
            bool result = _roomRepository.Add(new Room() { Name = name, Description = description });
            return result;
        }

        public bool SaveMappedRooms(string jsonResult)
        {
            var resultList = JsonConvert.DeserializeObject<List<RoomPlotTypeDetailsViewModel>>(jsonResult);

            try
            {
                foreach (var item in resultList)
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        var roomname = _db.Rooms.FirstOrDefault(x => x.Id == item.RoomId).Name;
                        var roomCount = _db.PlotTypeRoomMappings.Where(x => x.RoomId == item.RoomId && x.PlotTypeId == item.PlotTypeId).ToList().Count();
                        var roomstringcount = (roomCount == 0 && item.Count == 1) ? "" : " " + (roomCount + 1).ToString();

                        _plotTypeRoomMappingRepository.Add(new PlotTypeRoomMapping() { PlotTypeId = item.PlotTypeId, RoomId = item.RoomId, DisplayName = roomname + roomstringcount });

                        // Add room case for scond room work...
                        if(roomCount == 1)
                        {
                            var record = _db.PlotTypeRoomMappings.Where(x => x.RoomId == item.RoomId && x.PlotTypeId == item.PlotTypeId).FirstOrDefault();
                            record.DisplayName = roomname + " " + roomCount.ToString();

                            _db.Entry(record).State = EntityState.Modified;
                            _db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex) { return false; }
            return true;
        }

        public async Task<IActionResult> ManageProjects()
        {
            IEnumerable<Project> projects = await _projectRepository.GetAllProjects();
            IEnumerable<Plot> plots;

            List<ProjectPlotsViewModel> projectPlotsViewModelList = new List<ProjectPlotsViewModel>();

            foreach (Project project in projects)
            {
                ProjectPlotsViewModel projectPlotsViewModel = new ProjectPlotsViewModel();
                plots = await _plotRepository.GetAllPlotsByProject(project.Id);
                projectPlotsViewModel.ProjectId = project.Id;
                projectPlotsViewModel.Name = project.Name;
                projectPlotsViewModel.Description = project.Description;
                projectPlotsViewModel.Location = project.Location;
                projectPlotsViewModel.PlotsList = plots;

                projectPlotsViewModelList.Add(projectPlotsViewModel);
            }


            return View(projectPlotsViewModelList);
        }

        public async Task<IActionResult> ViewBuyers()
        {
            List<AppUserVM> appUserVMList = new List<AppUserVM>();

            IEnumerable<AppUser> appUsers = await _userManager.GetUsersInRoleAsync("home-buyer");

            appUsers = appUsers.OrderByDescending(x => x.UserName).ToList();

            foreach (AppUser appUser in appUsers)
            {
                AppUserVM appUserVM = new AppUserVM();

                var plots = await _plotRepository.GetAllPlotsByUser(appUser.Id);

                appUserVM.Id = appUser.Id;
                appUserVM.Name = appUser.UserName;
                appUserVM.Email = appUser.Email;
                appUserVM.Phone = appUser.PhoneNumber;
                appUserVM.PlotCount = plots.Count();
                appUserVM.Plots = plots;

                appUserVMList.Add(appUserVM);
            }

            return View(appUserVMList);
        }

        public bool AddProject(string projectName, string projectDescription, int location)
        {
            bool result = _projectRepository.Add(new Project() { Name = projectName, Description = projectDescription, LocationId = location });
            return result;
        }

        public bool AddPlot(int project, int plotType)
        {
            bool result = _plotRepository.Add(new Plot() { PlotTypeId = plotType, ProjectId = project });
            return result;
        }

        public bool AddBuyer(string buyer, int plotId)
        {
            Plot plot = _plotRepository.GetPlot(plotId);
            
            if (plot != null)
            {
                if (buyer == "0")
                {
                    plot.AppUserId = null;
                }
                else
                {
                    plot.AppUserId = buyer;
                }
                
            }

            bool result = _plotRepository.Update(plot);

            return result;
        }

        public async Task<JsonResult> GetAllPlotTypes()
        {
            var plotTypes = await _plotTypeRepository.GetAllPlotTypes();
            return Json(plotTypes);
        }

        public async Task<JsonResult> GetAllBuyers()
        {
            var buyers = await _userManager.GetUsersInRoleAsync("home-buyer");
            return Json(buyers);
        }

        public async Task<JsonResult> GetAllLocations()
        {
            var locations = await _locationRepository.GetAllLocations();
            return Json(locations);
        }

        public async Task<JsonResult> GetAllPlotByProject(int project)
        {
            var plots = await _plotRepository.GetAllPlotsByProject(project);
            return Json(plots);
        }

        public async Task<IActionResult> ManageCatalogues()
        {
            IEnumerable<Catalogue> catalogues = await _catalogueRepository.GetAllCatalogues();

            List<CatalogueViewModel> catalogueVMList = new List<CatalogueViewModel>();

            foreach (var catalogue in catalogues)
            {
                CatalogueViewModel catalogueVMItem = new CatalogueViewModel();

                catalogueVMItem.Id = catalogue.Id;
                catalogueVMItem.Name = catalogue.Name;
                catalogueVMItem.Description = catalogue.Description;
                catalogueVMItem.SingleOrMultiple = catalogue.SingleOrMultiple;
                catalogueVMItem.SubCatalogueItems = await _subCatalogueRepository.GetAllSubCatalogueItems(catalogue.Id);

                catalogueVMList.Add(catalogueVMItem);
            }

            return View(catalogueVMList);
        }

        public async Task<IActionResult> ManageSubCatalogues(int id)
        {
            IEnumerable<SubCatalogue> subCatalogues = await _subCatalogueRepository.GetAllSubCatalogueItems(id);

            List<SubCatalogueViewModel> subCataloguesVMList = new List<SubCatalogueViewModel>();

            foreach (var subCatalogue in subCatalogues)
            {
                SubCatalogueViewModel subCatalogueVMItem = new SubCatalogueViewModel();

                subCatalogueVMItem.Id = subCatalogue.Id;
                subCatalogueVMItem.Name = subCatalogue.Name;
                subCatalogueVMItem.Price = subCatalogue.Price;
                subCatalogueVMItem.Quantity = subCatalogue.Quantity;
                subCatalogueVMItem.CatalogueId = subCatalogue.CatalogueId;

                subCataloguesVMList.Add(subCatalogueVMItem);
            }


            var catalogueItem = await _catalogueRepository.GetCatalogueItem(id);

            ViewBag.catalogueId = catalogueItem.Id;
            ViewBag.catalogueName = catalogueItem.Name;


            return View(subCataloguesVMList);
        }

        public bool AddCatalogue(string catalogueName, string catalogueDescription, bool singleOrMultiple)
        {
            bool result = _catalogueRepository.Add(new Catalogue() { Name = catalogueName, Description = catalogueDescription, SingleOrMultiple = singleOrMultiple });
            return result;
        }

        public bool AddSubCatalogue(int catalogueId, string subCatalogueName, decimal subCataloguePrice)
        {
            bool result = _subCatalogueRepository.Add(new SubCatalogue() { Name = subCatalogueName, Price = subCataloguePrice, Quantity = 0, CatalogueId = catalogueId });

            return result;
        }
        public async Task<bool> UpdateSubCatalogue(int catalogueId,int subcatalogueId, decimal price)
        {
            try
            {
                var sub = _db.SubCatalogues.Where(x => x.Id == subcatalogueId).FirstOrDefault();
                sub.Price = price;
                _db.Entry(sub).State = EntityState.Modified;
                _db.SaveChanges();

                await NotifyPriceForPlotBuyers(catalogueId, subcatalogueId);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        public async Task<bool> UpdateStockSubCatalogue(int catalogueId, int subcatalogueId, int qty)
        {
            try
            {
                var sub = _db.SubCatalogues.Where(x => x.Id == subcatalogueId).FirstOrDefault();
                sub.Quantity = qty;
                _db.Entry(sub).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        

        private string GetRoomDetailsBasedOnMappedRooms(List<Room> mappedRooms)
        {
            string returnString = string.Empty;

            Dictionary<int, int> roomCounter = new Dictionary<int, int>();

            foreach (var room in mappedRooms)
            {
                if (roomCounter.ContainsKey(room.Id))
                {
                    roomCounter[room.Id]++;
                }
                else
                {
                    roomCounter.Add(room.Id, 1);
                }
            }

            foreach (var roomId in roomCounter.Keys)
            {
                string roomName = mappedRooms.FirstOrDefault(x => x.Id == roomId).Name;

                returnString += roomCounter[roomId] + " " + roomName + " ";

                if (roomId != roomCounter.Keys.Last()) returnString += ",";

            }

            return returnString;
        }

        public async Task<ActionResult> ManageChoicesExtras(int id = 0)
        {
            IEnumerable<PlotType> plotTypes = await _plotTypeRepository.GetAllPlotTypes();
            ViewBag.CurrentPloyType = id;
            return View(plotTypes);
        }

        public async Task<JsonResult> GetAllRoomsByPlotType(int plotType)
        {
            var rooms = await _plotTypeRoomMappingRepository.GetRoomMappingsByPlotType(plotType);

            var result = rooms.Select(r => new { id = r.RoomId, name = r.Room.Name }).Distinct().ToList();

            return Json(result);
        }

        public async Task<JsonResult> GetAllChoiceExtrasMappings(int plotType, int roomId)
        {
            var catalogues = await _catalogueRepository.GetAllCatalogues();

            var catalogueMappings = await _catalogueRoomPlotTypeMappingRepository.GetAllCatalogueMappingsByPlotTyeAndRoom(plotType, roomId);

            var result = new List<CatalogueListVM>();

            foreach (var item in catalogues)
            {
                CatalogueListVM catalogueListVM = new CatalogueListVM();

                var catalogueMapped = catalogueMappings.FirstOrDefault(x => x.CatalogueId == item.Id);

                catalogueListVM.CatalogueId = item.Id;
                catalogueListVM.CatalogueName = item.Name;
                catalogueListVM.IsActive = (catalogueMapped != null) ? catalogueMapped.IsActive : false;
                catalogueListVM.ChoiceOrExtra = (catalogueMapped != null) ? catalogueMapped.ChoiceorExtra : false;

                result.Add(catalogueListVM);
            }

            return Json(result);
        }

        public async Task<bool> SaveChoicesExtras(string jsonResult, int plotType, int roomId)
        {
            var resultList = JsonConvert.DeserializeObject<List<CatalogueListVM>>(jsonResult);

            var allMappings = await _catalogueRoomPlotTypeMappingRepository.GetAllCatalogueMappingsByPlotTyeAndRoom(plotType, roomId);

            try
            {
                foreach (var item in resultList)
                {

                    var exists = allMappings.Any(x => x.CatalogueId == item.CatalogueId);

                    if (exists)
                    {
                        var existingRecord = allMappings.FirstOrDefault(x => x.CatalogueId.Equals(item.CatalogueId));

                        existingRecord.IsActive = item.IsActive;
                        existingRecord.ChoiceorExtra = item.ChoiceOrExtra;

                        _catalogueRoomPlotTypeMappingRepository.Update(existingRecord);
                    }
                    else
                    {
                        CatalogueRoomPlotTypeMapping subCatalogueRoomPlotTypeMapping = new CatalogueRoomPlotTypeMapping();
                        subCatalogueRoomPlotTypeMapping.PlotTypeId = plotType;
                        subCatalogueRoomPlotTypeMapping.CatalogueId = item.CatalogueId;
                        subCatalogueRoomPlotTypeMapping.RoomId = roomId;
                        subCatalogueRoomPlotTypeMapping.IsActive = item.IsActive;
                        subCatalogueRoomPlotTypeMapping.ChoiceorExtra = item.ChoiceOrExtra;

                        _catalogueRoomPlotTypeMappingRepository.Add(subCatalogueRoomPlotTypeMapping);
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<IActionResult> AllOrders(int? plotid = null)
        {
            IEnumerable<SavedExtra> savedOrders = new List<SavedExtra>();

            if (plotid == null)
            {
                savedOrders = await _savedExtraRepository.GetSavedExtrasAsOrders();
                savedOrders = savedOrders.OrderByDescending(y => y.Transaction.Date).ToList();

            }
            else
            {
                savedOrders = await _savedExtraRepository.GetSavedExtrasByPlot(plotid);
                savedOrders = savedOrders.Where(x => x.TransactionId != null).OrderByDescending(y => y.Transaction.Date).ToList();
            }

            savedOrders = savedOrders.DistinctBy(x => x.TransactionId).ToList();

            return View(savedOrders);

        }
        public async Task<IActionResult> Dashboard()
        {
            DashboardVM dashboardVM = new DashboardVM();

            var buyer_role = _db.Roles.Where(x => x.Name == "home-buyer").FirstOrDefault().Id;
            var buyers = _db.UserRoles.Where(x => x.RoleId == buyer_role).Select(x => x.UserId).ToList();

            var projectsCount = _db.Projects.Count();
            var plotsCount = _db.Plots.Count();
            var buyersCount = _db.Users.Where(x => buyers.Contains(x.Id)).Count();
            var cataloguesCount = _db.Catalogues.Count();

            var unsoldPlots = _db.Plots.Where(x => x.AppUserId == null).Count();
            var soldPlots = _db.Plots.Where(x => x.AppUserId != null).Count();

            var locations = _db.Locations.ToList();
            List<LocationPlotsVM> locationPlotsVM = new List<LocationPlotsVM>();

            foreach (var item in locations)
            {
                var count = _db.Plots.Include(x => x.Project).Where(x => x.Project.LocationId == item.Id).Count();
                locationPlotsVM.Add(new LocationPlotsVM { Name = item.Name, Count = count });
            }

            dashboardVM.BuyersCount = buyersCount;
            dashboardVM.ProjectsCount = projectsCount;
            dashboardVM.PlotsCount = plotsCount;
            dashboardVM.CatalogueCount = cataloguesCount;
            dashboardVM.UnSoldCount = unsoldPlots;
            dashboardVM.SoldCount = soldPlots;
            dashboardVM.LocationPlotsVM = locationPlotsVM;

            return View(dashboardVM);
        }

        public async Task<IActionResult> ChoicesExtras(int plotid)
        {

            Plot plot = _plotRepository.GetPlot(plotid);

            List<PlotRoomChoiceExtraVM> plotRoomChoiceExtraVMList = new List<PlotRoomChoiceExtraVM>();

            IEnumerable<PlotTypeRoomMapping> roomMappings = await _plotTypeRoomMappingRepository.GetRoomMappingsByPlotType(plot.PlotTypeId);

            roomMappings = roomMappings.OrderBy(x=>x.RoomId).ToList(); // Sorted as per the room pho

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

        public async Task<bool> DeleteBuyer(string buyerid)
        {
            try
            {
                var buyer = _db.Users.Where(x => x.Id == buyerid).FirstOrDefault();
                _db.Entry(buyer).State = EntityState.Deleted;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<IActionResult> BuyerPlots(string buyerid)
        {
            var plots = await _plotRepository.GetAllPlotsByUser(buyerid);
            return View(plots);
        }
        public async Task<IActionResult> BuyerOrders(int plotid)
        {
            PlotOrderVM plotOrderVM = new PlotOrderVM();

            PlotRoomVM plotRoomVM = new PlotRoomVM();

            Plot plot = _plotRepository.GetPlot(plotid);

            var savedExtras = await _savedExtraRepository.GetSavedExtrasByPlot(plotid);

            savedExtras = savedExtras.Where(x => x.TransactionId != null).OrderByDescending(y => y.Transaction.Date).ToList();

            savedExtras = savedExtras.DistinctBy(x => x.TransactionId).ToList();

            plotOrderVM.Plot = plot;
            plotOrderVM.SavedExtras = savedExtras;

            return View(plotOrderVM);
        }

        public async Task<bool> SendNotification(int plotid, string title, string message)
        {
            try
            {
                Notification notification = new Notification();

                notification.Title = title;
                notification.Message = message;
                notification.MessageDate = DateTime.Now;
                notification.PlotId = plotid;

                _db.Entry(notification).State = EntityState.Added;
                _db.SaveChanges();

                Plot plot = _plotRepository.GetPlot(plotid);
                var userEmail = plot.AppUser.Email;

                await _emailService.SendEmailAsync(userEmail, title, message);

            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> NotifyPlotBuyers(int plotTypeId)
        {
            try
            {
                IEnumerable<Plot> plots = await _plotRepository.GetAllPlotsByPlotType(plotTypeId);

                plots = plots.Where(x => x.AppUserId != null);

                foreach (var plot in plots)
                {
                    await SendNotification(plot.Id, "Change Notification - Plot PL" + plot.Id, "New Changes made to current Choices & Extras. Please check the your plot manager page.");
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;


        }
        public async Task<bool> NotifyPriceForPlotBuyers(int catalogueId, int subcatalogueId)
        {
            try
            {
                var sub = _db.SubCatalogues.Where(x => x.Id == subcatalogueId).FirstOrDefault();

                var catalogue = _db.Catalogues.Where(x => x.Id == catalogueId).FirstOrDefault();

                var plotTypes = _db.CatalogueRoomPlotTypeMappings.Where(x=>x.CatalogueId ==  catalogueId).Select(x=>x.PlotTypeId).ToList();

                var plots = _db.Plots.Include(x=>x.AppUser).Where(x=> plotTypes.Contains(x.PlotTypeId)).ToList();

                plots = plots.Where(x => x.AppUserId != null).DistinctBy(x => x.AppUserId).ToList();

                foreach (var plot in plots)
                {
                    await SendNotification(plot.Id, "Price Change Notification - Plot PL" + plot.Id, "Price Changed for " + catalogue.Name + " - " + sub.Name);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;


        }


        public async Task<IActionResult> AllCataloguesStock()
        {
            IEnumerable<SubCatalogue> subCatalogues = await _subCatalogueRepository.GetAllListSubCatalogueItems();

            List<SubCatalogueViewModel> subCataloguesVMList = new List<SubCatalogueViewModel>();

            foreach (var subCatalogue in subCatalogues)
            {
                SubCatalogueViewModel subCatalogueVMItem = new SubCatalogueViewModel();

                subCatalogueVMItem.Id = subCatalogue.Id;
                subCatalogueVMItem.Name = subCatalogue.Name;
                subCatalogueVMItem.Price = subCatalogue.Price;
                subCatalogueVMItem.Quantity = subCatalogue.Quantity;
                subCatalogueVMItem.CatalogueId = subCatalogue.CatalogueId;
                subCatalogueVMItem.Catalogue = subCatalogue.Catalogue;

                subCataloguesVMList.Add(subCatalogueVMItem);
            }

            subCataloguesVMList = subCataloguesVMList.OrderBy(x=>x.Catalogue.Name).ToList();

            return View(subCataloguesVMList);
        }

        public async Task<bool> EmailSupplier(string email, string title, string message)
        {
            try
            {
                await _emailService.SendEmailAsync(email, title, message);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        
    }
}
