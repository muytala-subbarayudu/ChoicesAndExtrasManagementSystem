using ChoicesExtrasManagement.Data;
using ChoicesExtrasManagement.Interfaces;
using ChoicesExtrasManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ChoicesExtrasManagement.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ChoicesExtrasManagementDbContext _context;
        public ProjectRepository(ChoicesExtrasManagementDbContext context)
        {
            _context = context;
        }

        public bool Add(Project project)
        {
            _context.Add(project);
            return Save();
        }

        public bool Delete(Project project)
        {
            _context.Remove(project);
            return Save();
        }

        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            return await _context.Projects.Include(l=>l.Location).ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetAllProjectsByLocation(int location)
        {
            return await _context.Projects.Where(l => l.LocationId == location).ToListAsync();
        }

        public async Task<Project> GetByProjectsIdAsync(int id)
        {
            return await _context.Projects.Include(l=>l.Location).FirstOrDefaultAsync(i=>i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Project project)
        {
            _context.Update(project);
            return Save();
        }
    }
}
