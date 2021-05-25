using Convey.Persistence.MongoDB;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Infrastructure.Mongo.Repositories
{
    internal sealed class ProjectRepository : IProjectRepository
    {
        private readonly IMongoRepository<ProjectDocument, string> _repository;

        public ProjectRepository(IMongoRepository<ProjectDocument, string> repository)
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
