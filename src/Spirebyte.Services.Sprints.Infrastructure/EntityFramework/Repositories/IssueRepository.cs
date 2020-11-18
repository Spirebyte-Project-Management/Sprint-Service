using System;
using System.Threading.Tasks;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Repositories
{
    internal sealed class IssueRepository : IIssueRepository
    {
        private readonly IEfRepository<SprintsDbContext, IssueTable, Guid> _repository;

        public IssueRepository(IEfRepository<SprintsDbContext, IssueTable, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<Issue> GetAsync(Guid id)
        {
            var issue = await _repository.GetAsync(id);

            return issue?.AsEntity();
        }
        public Task<bool> ExistsAsync(Guid id) => _repository.ExistsAsync(c => c.Id == id);
        public Task<bool> ExistsAsync(string key) => _repository.ExistsAsync(c => c.Key == key);
        public Task AddAsync(Issue issue) => _repository.AddAsync(issue.AsDocument());
    }
}
