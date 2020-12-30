using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Repositories
{
    internal sealed class IssueRepository : IIssueRepository
    {
        private readonly IEfRepository<SprintsDbContext, IssueTable, string> _repository;

        public IssueRepository(IEfRepository<SprintsDbContext, IssueTable, string> repository)
        {
            _repository = repository;
        }
        public async Task<Issue> GetAsync(string id)
        {
            var issue = await _repository.GetAsync(id);

            return issue?.AsEntity();
        }
        public Task<bool> ExistsAsync(string id) => _repository.ExistsAsync(c => c.Id == id);
        public Task AddAsync(Issue issue) => _repository.AddAsync(issue.AsDocument());
        public async Task UpdateAsync(Issue issue)
        {
            var issueEntity = await _repository.GetAsync(issue.Id);

            issueEntity.SprintId = issue.SprintId;
            issueEntity.ProjectId = issue.ProjectId;

            await _repository.SaveChangesAsync();
        }

        public Task SaveChangesAsync() => _repository.SaveChangesAsync();

    }
}
