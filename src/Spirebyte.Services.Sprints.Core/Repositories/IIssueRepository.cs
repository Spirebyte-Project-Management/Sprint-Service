using Spirebyte.Services.Sprints.Core.Entities;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Core.Repositories
{
    public interface IIssueRepository
    {
        Task<Issue> GetAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task AddAsync(Issue issue);
        Task UpdateAsync(Issue issue);
        Task SaveChangesAsync();
    }
}
