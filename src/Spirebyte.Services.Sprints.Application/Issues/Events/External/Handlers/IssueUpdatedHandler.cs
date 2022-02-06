using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Spirebyte.Services.Sprints.Application.Issues.Exceptions;
using Spirebyte.Services.Sprints.Application.Sprints.Exceptions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Enums;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Issues.Events.External.Handlers;

internal sealed class IssueUpdatedHandler : IEventHandler<IssueUpdated>
{
    private readonly IIssueRepository _issueRepository;
    private readonly ISprintRepository _sprintRepository;

    public IssueUpdatedHandler(IIssueRepository issueRepository, ISprintRepository sprintRepository)
    {
        _issueRepository = issueRepository;
        _sprintRepository = sprintRepository;
    }

    public async Task HandleAsync(IssueUpdated @event, CancellationToken cancellationToken = default)
    {
        if (!await _issueRepository.ExistsAsync(@event.IssueId)) throw new IssueNotFoundException(@event.IssueId);

        var issue = await _issueRepository.GetAsync(@event.IssueId);
        var updatedIssue = new Issue(issue.Id, issue.ProjectId, issue.SprintId, @event.StoryPoints, @event.Status);

        if (issue.SprintId is not null)
        {
            if (!await _sprintRepository.ExistsAsync(issue.SprintId))
                throw new SprintNotFoundException(issue.SprintId);
            var sprint = await _sprintRepository.GetAsync(issue.SprintId);

            if (issue.StoryPoints != updatedIssue.StoryPoints) sprint.IssueChanged(updatedIssue, issue);

            if (issue.Status != updatedIssue.Status)
            {
                if (issue.Status == IssueStatus.DONE && updatedIssue.Status != IssueStatus.DONE)
                    sprint.IssueReOpened(updatedIssue);

                if (issue.Status != IssueStatus.DONE && updatedIssue.Status == IssueStatus.DONE)
                    sprint.IssueCompleted(updatedIssue);
            }

            await _sprintRepository.UpdateAsync(sprint);
        }

        await _issueRepository.UpdateAsync(updatedIssue);
    }
}