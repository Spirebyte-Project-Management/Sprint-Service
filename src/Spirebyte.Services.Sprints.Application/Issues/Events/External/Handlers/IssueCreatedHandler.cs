using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Spirebyte.Services.Sprints.Application.Issues.Exceptions;
using Spirebyte.Services.Sprints.Application.Projects.Exceptions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Enums;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Issues.Events.External.Handlers;

internal sealed class IssueCreatedHandler : IEventHandler<IssueCreated>
{
    private readonly IIssueRepository _issueRepository;
    private readonly IProjectRepository _projectRepository;

    public IssueCreatedHandler(IIssueRepository issueRepository, IProjectRepository projectRepository)
    {
        _issueRepository = issueRepository;
        _projectRepository = projectRepository;
    }

    public async Task HandleAsync(IssueCreated @event, CancellationToken cancellationToken = default)
    {
        if (!await _projectRepository.ExistsAsync(@event.ProjectId))
            throw new ProjectNotFoundException(@event.ProjectId);

        if (await _issueRepository.ExistsAsync(@event.Id)) throw new IssueAlreadyCreatedException(@event.Id);

        var issue = new Issue(@event.Id, @event.ProjectId, null, @event.StoryPoints, IssueStatus.TODO);
        await _issueRepository.AddAsync(issue);
    }
}