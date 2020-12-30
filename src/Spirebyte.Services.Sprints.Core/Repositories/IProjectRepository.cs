using Spirebyte.Services.Sprints.Core.Entities;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Core.Repositories
{
    public interface IProjectRepository
    {
        Task<Project> GetAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task AddAsync(Project project);
    }
}
