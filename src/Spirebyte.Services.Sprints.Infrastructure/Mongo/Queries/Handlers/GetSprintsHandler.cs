using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Sprints.Application.DTO;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Application.Queries;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Infrastructure.Mongo.Queries.Handlers
{
    internal sealed class GetSprintsHandler : IQueryHandler<GetSprints, IEnumerable<SprintDto>>
    {
        private readonly IMongoRepository<SprintDocument, string> _sprintRepository;
        private readonly IMongoRepository<ProjectDocument, string> _projectRepository;

        public GetSprintsHandler(IMongoRepository<SprintDocument, string> sprintRepository,
            IMongoRepository<ProjectDocument, string> projectRepository)
        {
            _sprintRepository = sprintRepository;
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<SprintDto>> HandleAsync(GetSprints query)
        {
            var documents = _sprintRepository.Collection.AsQueryable();

            if (string.IsNullOrWhiteSpace(query.ProjectId))
            {
                return new List<SprintDto>();
            }

            if (!await _projectRepository.ExistsAsync(p => p.Id == query.ProjectId))
            {
                throw new ProjectNotFoundException(query.ProjectId);
            }

            var project = await _projectRepository.GetAsync(query.ProjectId);

            var sprintsWithProject = await documents.Where(p => p.ProjectId == project.Id).ToListAsync();

            return sprintsWithProject.Select(p => p.AsDto());
        }
    }
}
