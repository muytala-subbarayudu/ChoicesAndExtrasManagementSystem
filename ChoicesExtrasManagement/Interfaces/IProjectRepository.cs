using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllProjects();

        Task<Project> GetByProjectsIdAsync(int id);

        Task<IEnumerable<Project>> GetAllProjectsByLocation(int location);
        bool Add(Project project);
        bool Update(Project project);
        bool Delete(Project project);
        bool Save();
    }
}
