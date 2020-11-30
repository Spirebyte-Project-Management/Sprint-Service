using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Repositories
{
    internal sealed class ProjectRepository : IProjectRepository
    {
        private readonly IEfRepository<SprintsDbContext, ProjectTable, Guid> _repository;

        public ProjectRepository(IEfRepository<SprintsDbContext, ProjectTable, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<Project> GetAsync(Guid id)
        {
            var project = await _repository.GetAsync(id);

            return project?.AsEntity();
        }
        public async Task<string> GetKeyAsync(Guid projectId)
        {
            var project = await _repository.GetAsync(projectId);

            return project.Key;
        }
        public Task<bool> ExistsAsync(Guid id) => _repository.ExistsAsync(c => c.Id == id);
        public Task<bool> ExistsAsync(string key) => _repository.ExistsAsync(c => c.Key == key);
        public Task AddAsync(Project project) => _repository.AddAsync(project.AsDocument());
    }
}
