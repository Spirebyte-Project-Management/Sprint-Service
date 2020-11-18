using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Repositories
{
    internal sealed class SprintRepository : ISprintRepository
    {
        private readonly IEfRepository<SprintsDbContext, SprintTable, Guid> _repository;

        public SprintRepository(IEfRepository<SprintsDbContext, SprintTable, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Sprint> GetAsync(Guid sprintId)
        {
            var sprint = await _repository.GetAsync(x => x.Id == sprintId);

            return sprint?.AsEntity();
        }

        public async Task<Sprint> GetAsync(string sprintKey)
        {
            var sprint = await _repository.GetAsync(x => x.Key == sprintKey);

            return sprint?.AsEntity();
        }

        public async Task<int> GetSprintCountOfProjectAsync(Guid projectId) => await _repository.Collection.CountAsync(x => x.ProjectId == projectId);
        public Task<bool> ExistsWithKeyAsync(string key) => _repository.ExistsAsync(c => c.Key == key);

        public Task AddAsync(Sprint sprint) => _repository.AddAsync(sprint.AsDocument());

        public Task UpdateAsync(Sprint sprint) => _repository.UpdateAsync(sprint.AsDocument());
    }
}
