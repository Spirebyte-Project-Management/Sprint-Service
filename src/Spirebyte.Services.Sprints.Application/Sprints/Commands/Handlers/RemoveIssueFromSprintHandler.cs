using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Sprints.Application.Issues.Exceptions;
using Spirebyte.Services.Sprints.Application.Services.Interfaces;
using Spirebyte.Services.Sprints.Application.Sprints.Events;
using Spirebyte.Services.Sprints.Application.Sprints.Exceptions;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Sprints.Commands.Handlers;

// Simple wrapper
internal sealed class RemoveIssueFromSprintHandler : ICommandHandler<RemoveIssueFromSprint>
{
    private readonly IIssueRepository _issueRepository;
    private readonly IMessageBroker _messageBroker;
    private readonly ISprintRepository _sprintRepository;

    public RemoveIssueFromSprintHandler(ISprintRepository sprintRepository,
        IIssueRepository issueRepository,
        IMessageBroker messageBroker)
    {
        _sprintRepository = sprintRepository;
        _issueRepository = issueRepository;
        _messageBroker = messageBroker;
    }

    public async Task HandleAsync(RemoveIssueFromSprint command, CancellationToken cancellationToken = default)
    {
        if (!await _sprintRepository.ExistsAsync(command.SprintId)) throw new SprintNotFoundException(command.SprintId);

        if (!await _issueRepository.ExistsAsync(command.IssueId)) throw new IssueNotFoundException(command.IssueId);
        var sprint = await _sprintRepository.GetAsync(command.SprintId);
        var issue = await _issueRepository.GetAsync(command.IssueId);

        issue.RemoveFromSprint();
        await _issueRepository.UpdateAsync(issue);

        sprint.RemoveIssue(issue);
        await _sprintRepository.UpdateAsync(sprint);


        await _messageBroker.PublishAsync(new RemovedIssueFromSprint(sprint.Id, sprint.ProjectId, issue.Id));
    }
}