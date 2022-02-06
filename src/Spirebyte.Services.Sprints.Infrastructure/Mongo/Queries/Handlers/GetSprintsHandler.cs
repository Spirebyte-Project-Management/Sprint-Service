using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Sprints.Application.Projects.Exceptions;
using Spirebyte.Services.Sprints.Application.Sprints.DTO;
using Spirebyte.Services.Sprints.Application.Sprints.Queries;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Sprints.Infrastructure.Mongo.Queries.Handlers;

internal sealed class GetSprintsHandler : IQueryHandler<GetSprints, IEnumerable<SprintDto>>
{
    private readonly IMongoRepository<ProjectDocument, string> _projectRepository;
    private readonly IMongoRepository<SprintDocument, string> _sprintRepository;

    public GetSprintsHandler(IMongoRepository<SprintDocument, string> sprintRepository,
        IMongoRepository<ProjectDocument, string> projectRepository)
    {
        _sprintRepository = sprintRepository;
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<SprintDto>> HandleAsync(GetSprints query,
        CancellationToken cancellationToken = default)
    {
        var documents = _sprintRepository.Collection.AsQueryable();

        if (string.IsNullOrWhiteSpace(query.ProjectId)) return new List<SprintDto>();

        if (!await _projectRepository.ExistsAsync(p => p.Id == query.ProjectId))
            throw new ProjectNotFoundException(query.ProjectId);

        var project = await _projectRepository.GetAsync(query.ProjectId);

        var sprintsWithProject = await documents.Where(p => p.ProjectId == project.Id).ToListAsync();

        return sprintsWithProject.Select(p => p.AsDto());
    }
}