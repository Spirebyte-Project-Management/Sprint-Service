using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Repositories
{
    internal sealed class ProjectRepository : IProjectRepository
    {
        private readonly IEfRepository<SprintsDbContext, ProjectTable, string> _repository;

        public ProjectRepository(IEfRepository<SprintsDbContext, ProjectTable, string> repository)
        {
            _repository = repository;
        }
        public async Task<Project> GetAsync(string id)
        {
            var project = await _repository.GetAsync(id);

            return project?.AsEntity();
        }
        public Task<bool> ExistsAsync(string id) => _repository.ExistsAsync(c => c.Id == id);
        public Task AddAsync(Project project) => _repository.AddAsync(project.AsDocument());
    }
}
