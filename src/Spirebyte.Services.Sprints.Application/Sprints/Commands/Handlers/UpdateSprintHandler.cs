using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Sprints.Application.Sprints.Events;
using Spirebyte.Services.Sprints.Application.Sprints.Exceptions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Sprints.Commands.Handlers;

internal sealed class UpdateSprintHandler : ICommandHandler<UpdateSprint>
{
    private readonly IMessageBroker _messageBroker;
    private readonly IProjectRepository _projectRepository;
    private readonly ISprintRepository _sprintRepository;

    public UpdateSprintHandler(IProjectRepository projectRepository, ISprintRepository sprintRepository,
        IMessageBroker messageBroker)
    {
        _projectRepository = projectRepository;
        _sprintRepository = sprintRepository;
        _messageBroker = messageBroker;
    }

    public async Task HandleAsync(UpdateSprint command, CancellationToken cancellationToken = default)
    {
        var sprint = await _sprintRepository.GetAsync(command.Id);
        if (sprint is null) throw new SprintNotFoundException(command.Id);

        var newSprint = new Sprint(sprint.Id, command.Title, command.Description, sprint.ProjectId, sprint.IssueIds,
            sprint.CreatedAt, sprint.StartedAt, command.StartDate, command.EndDate, sprint.EndedAt, sprint.Changes,
            sprint.RemainingStoryPoints, sprint.TotalStoryPoints);
        await _sprintRepository.UpdateAsync(newSprint);

        await _messageBroker.SendAsync(new SprintUpdated(newSprint, sprint), cancellationToken);
    }
}