using System;
using System.Threading.Tasks;
using Spirebyte.Services.Sprints.Core.Entities;

namespace Spirebyte.Services.Sprints.Core.Repositories
{
    public interface IIssueRepository
    {
        Task<Issue> GetAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsAsync(string key);
        Task AddAsync(Issue issue);
    }
}
