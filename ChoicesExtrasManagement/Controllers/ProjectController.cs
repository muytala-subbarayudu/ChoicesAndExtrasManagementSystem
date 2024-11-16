using ChoicesExtrasManagement.Interfaces;
using ChoicesExtrasManagement.Models;
using ChoicesExtrasManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChoicesExtrasManagement.Controllers
{
    public class ProjectController : Controller
    {
        public readonly IProjectRepository _projectRepository;
        public readonly IPlotRepository _plotRepository;
        public readonly ILocationRepository _locationRepository;


        public ProjectController(IProjectRepository projectRepository, IPlotRepository plotRepository,
            ILocationRepository locationRepository)
        {
            _projectRepository = projectRepository;
            _plotRepository = plotRepository;
            _locationRepository = locationRepository;
        }
        public async Task<IActionResult> Index(int? location = null)
        {
            IEnumerable<Project> projects = await _projectRepository.GetAllProjects();

            if(location != null)
            {
                projects = projects.Where(x => x.LocationId == location);
                
            }

            ProjectsViewModel projectsVM = new ProjectsViewModel();

            projectsVM.Projects = projects;
            projectsVM.AllLocations = await _locationRepository.GetAllLocations();

            if (location != null)
            {
                projectsVM.currentLocation = location;
            }


            return View(projectsVM);
        }

        public async Task<IActionResult> Detail(int id)
        {
            ProjectPlotsViewModel projectPlotsViewModel = new ProjectPlotsViewModel();

            Project project = await _projectRepository.GetByProjectsIdAsync(id);
            IEnumerable<Plot> plots = await _plotRepository.GetAllPlotsByProject(id);

            projectPlotsViewModel.Name = project.Name;
            projectPlotsViewModel.Description = project.Description;
            projectPlotsViewModel.Location = project.Location;
            projectPlotsViewModel.PlotsList = plots;

            return View(projectPlotsViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }


    }
}
