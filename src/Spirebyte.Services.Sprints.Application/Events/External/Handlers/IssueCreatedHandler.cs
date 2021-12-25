using System.Threading.Tasks;
using Convey.CQRS.Events;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Events.External.Handlers;

public class SignedUpHandler : IEventHandler<IssueCreated>
{
    private readonly IIssueRepository _issueRepository;
    private readonly IProjectRepository _projectRepository;

    public SignedUpHandler(IIssueRepository issueRepository, IProjectRepository projectRepository)
    {
        _issueRepository = issueRepository;
        _projectRepository = projectRepository;
    }

    public async Task HandleAsync(IssueCreated @event)
    {
        if (!await _projectRepository.ExistsAsync(@event.ProjectId))
            throw new ProjectNotFoundException(@event.ProjectId);

        if (await _issueRepository.ExistsAsync(@event.IssueId)) throw new IssueAlreadyCreatedException(@event.IssueId);

        var issue = new Issue(@event.IssueId, @event.ProjectId, null);
        await _issueRepository.AddAsync(issue);
    }
}