using Spirebyte.Services.Sprints.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Core.Repositories
{
    public interface IIssueRepository
    {
        Task<Issue> GetAsync(Guid id);
        Task<Issue> GetAsync(string sprintKey);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsAsync(string key);
        Task AddAsync(Issue issue);
        Task UpdateAsync(Issue issue);
        Task SaveChangesAsync();
    }
}
