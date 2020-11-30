using Spirebyte.Services.Sprints.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Core.Repositories
{
    public interface ISprintRepository
    {
        Task<Sprint> GetAsync(Guid sprintId);
        Task<Sprint> GetAsync(string sprintKey);
        Task<int> GetSprintCountOfProjectAsync(Guid projectId);
        Task<bool> ExistsAsync(string key);
        Task AddAsync(Sprint sprint);
        Task UpdateAsync(Sprint sprint);
    }
}
