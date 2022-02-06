using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Spirebyte.Services.Sprints.Application.Sprints.DTO;
using Spirebyte.Services.Sprints.Application.Sprints.Queries;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Sprints.Infrastructure.Mongo.Queries.Handlers;

internal sealed class GetSprintHandler : IQueryHandler<GetSprint, SprintDto>
{
    private readonly IMongoRepository<SprintDocument, string> _sprintRepository;

    public GetSprintHandler(IMongoRepository<SprintDocument, string> sprintRepository)
    {
        _sprintRepository = sprintRepository;
    }

    public async Task<SprintDto> HandleAsync(GetSprint query, CancellationToken cancellationToken = default)
    {
        var sprint = await _sprintRepository.GetAsync(query.SprintId);

        return sprint?.AsDto();
    }
}