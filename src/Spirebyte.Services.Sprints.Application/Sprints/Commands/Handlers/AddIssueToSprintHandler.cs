using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Sprints.Application.Issues.Exceptions;
using Spirebyte.Services.Sprints.Application.Sprints.Events;
using Spirebyte.Services.Sprints.Application.Sprints.Exceptions;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Sprints.Commands.Handlers;

internal sealed class AddIssueToSprintHandler : ICommandHandler<AddIssueToSprint>
{
    private readonly IIssueRepository _issueRepository;
    private readonly ILogger<AddIssueToSprintHandler> _logger;
    private readonly IMessageBroker _messageBroker;
    private readonly ISprintRepository _sprintRepository;

    public AddIssueToSprintHandler(ISprintRepository sprintRepository,
        IIssueRepository issueRepository,
        IMessageBroker messageBroker,
        ILogger<AddIssueToSprintHandler> logger)
    {
        _sprintRepository = sprintRepository;
        _issueRepository = issueRepository;
        _messageBroker = messageBroker;
        _logger = logger;
    }

    public async Task HandleAsync(AddIssueToSprint command, CancellationToken cancellationToken = default)
    {
        if (!await _sprintRepository.ExistsAsync(command.SprintId)) throw new SprintNotFoundException(command.SprintId);

        if (!await _issueRepository.ExistsAsync(command.IssueId)) throw new IssueNotFoundException(command.IssueId);

        var sprint = await _sprintRepository.GetAsync(command.SprintId);
        var issue = await _issueRepository.GetAsync(command.IssueId);

        issue.AddToSprint(sprint.Id);
        await _issueRepository.UpdateAsync(issue);

        sprint.AddIssue(issue);
        await _sprintRepository.UpdateAsync(sprint);

        await _messageBroker.SendAsync(new AddedIssueToSprint(sprint.Id, sprint.ProjectId, issue.Id), cancellationToken);
    }
}