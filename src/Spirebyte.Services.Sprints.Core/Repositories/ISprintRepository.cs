using Spirebyte.Services.Sprints.Core.Entities;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Core.Repositories
{
    public interface ISprintRepository
    {
        Task<Sprint> GetAsync(string sprintId);
        Task<long> GetSprintCountOfProjectAsync(string projectId);
        Task<Sprint> GetLatest();
        Task<bool> ExistsAsync(string sprintId);
        Task AddAsync(Sprint sprint);
        Task UpdateAsync(Sprint sprint);
    }
}
