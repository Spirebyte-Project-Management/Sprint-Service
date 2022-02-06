using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Sprints.Application.Issues.Queries;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;

namespace Spirebyte.Services.Sprints.Infrastructure.Mongo.Queries.Handlers;

internal sealed class
    GetIssuesWithoutSprintForProjectHandler : IQueryHandler<GetIssuesWithoutSprintForProject, string[]>
{
    private readonly IMongoRepository<IssueDocument, string> _issueRepository;
    private readonly IMongoRepository<ProjectDocument, string> _projectRepository;

    public GetIssuesWithoutSprintForProjectHandler(IMongoRepository<IssueDocument, string> issueRepository,
        IMongoRepository<ProjectDocument, string> projectRepository)
    {
        _issueRepository = issueRepository;
        _projectRepository = projectRepository;
    }

    public async Task<string[]> HandleAsync(GetIssuesWithoutSprintForProject query,
        CancellationToken cancellationToken = default)
    {
        var documents = _issueRepository.Collection.AsQueryable();
        var project = await _projectRepository.GetAsync(query.ProjectId);

        return documents.Where(i => i.ProjectId == project.Id && (i.SprintId == null || i.SprintId == string.Empty))
            .Select(i => i.Id).ToArray();
    }
}