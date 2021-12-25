using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Sprints.Application.Events;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Application.Services.Interfaces;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Commands.Handlers;

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

    public async Task HandleAsync(RemoveIssueFromSprint command)
    {
        if (!await _sprintRepository.ExistsAsync(command.SprintId)) throw new SprintNotFoundException(command.SprintId);

        if (!await _issueRepository.ExistsAsync(command.IssueId)) throw new IssueNotFoundException(command.IssueId);

        var issue = await _issueRepository.GetAsync(command.IssueId);
        issue.RemoveFromSprint();

        await _issueRepository.UpdateAsync(issue);

        await _messageBroker.PublishAsync(new RemovedIssueFromSprint(command.SprintId, issue.Id));
    }
}