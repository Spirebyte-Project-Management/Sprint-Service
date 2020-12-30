using System.Linq;
using Microsoft.EntityFrameworkCore;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Repositories
{
    internal sealed class SprintRepository : ISprintRepository
    {
        private readonly IEfRepository<SprintsDbContext, SprintTable, string> _repository;

        public SprintRepository(IEfRepository<SprintsDbContext, SprintTable, string> repository)
        {
            _repository = repository;
        }

        public async Task<Sprint> GetAsync(string sprintId)
        {
            var sprint = await _repository.GetAsync(x => x.Id == sprintId);

            return sprint?.AsEntity();
        }

        public async Task<Sprint> GetLatest()
        {
            var documents = _repository.Collection.AsQueryable();

            var sprint = await documents.OrderByDescending(c => c.CreatedAt).FirstOrDefaultAsync();
            return sprint.AsEntity();
        }

        public async Task<int> GetSprintCountOfProjectAsync(string projectId) => await _repository.Collection.CountAsync(x => x.ProjectId == projectId);

        public Task<bool> ExistsAsync(string sprintId) => _repository.ExistsAsync(c => c.Id == sprintId);

        public Task AddAsync(Sprint sprint) => _repository.AddAsync(sprint.AsDocument());

        public Task UpdateAsync(Sprint sprint) => _repository.UpdateAsync(sprint.AsDocument());
    }
}
