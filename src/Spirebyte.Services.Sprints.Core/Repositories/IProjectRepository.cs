using Spirebyte.Services.Sprints.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Core.Repositories
{
    public interface IProjectRepository
    {
        Task<Project> GetAsync(Guid id);
        Task<string> GetKeyAsync(Guid projectId);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsAsync(string key);
        Task AddAsync(Project project);
    }
}
