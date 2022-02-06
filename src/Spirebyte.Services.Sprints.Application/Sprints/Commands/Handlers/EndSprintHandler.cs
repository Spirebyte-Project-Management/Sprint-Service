using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Sprints.Application.Services.Interfaces;
using Spirebyte.Services.Sprints.Application.Sprints.Events;
using Spirebyte.Services.Sprints.Application.Sprints.Exceptions;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Sprints.Commands.Handlers;

internal sealed class EndSprintHandler : ICommandHandler<EndSprint>
{
    private readonly IMessageBroker _messageBroker;
    private readonly ISprintRepository _sprintRepository;

    public EndSprintHandler(ISprintRepository sprintRepository,
        IMessageBroker messageBroker)
    {
        _sprintRepository = sprintRepository;
        _messageBroker = messageBroker;
    }

    public async Task HandleAsync(EndSprint command, CancellationToken cancellationToken = default)
    {
        if (!await _sprintRepository.ExistsAsync(command.Id)) throw new SprintNotFoundException(command.Id);

        var sprint = await _sprintRepository.GetAsync(command.Id);

        if (sprint.StartedAt == DateTime.MinValue) throw new SprintNotStartedException(sprint.Id);

        sprint.End();
        await _sprintRepository.UpdateAsync(sprint);

        await _messageBroker.PublishAsync(new EndedSprint(sprint.Id));
    }
}