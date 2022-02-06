using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Sprints.Application.Services.Interfaces;
using Spirebyte.Services.Sprints.Application.Sprints.Events;
using Spirebyte.Services.Sprints.Application.Sprints.Exceptions;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Sprints.Commands.Handlers;

internal sealed class StartSprintHandler : ICommandHandler<StartSprint>
{
    private readonly IMessageBroker _messageBroker;
    private readonly ISprintRepository _sprintRepository;

    public StartSprintHandler(ISprintRepository sprintRepository,
        IMessageBroker messageBroker)
    {
        _sprintRepository = sprintRepository;
        _messageBroker = messageBroker;
    }

    public async Task HandleAsync(StartSprint command, CancellationToken cancellationToken = default)
    {
        if (!await _sprintRepository.ExistsAsync(command.Id)) throw new SprintNotFoundException(command.Id);

        var sprint = await _sprintRepository.GetAsync(command.Id);
        sprint.Start();
        await _sprintRepository.UpdateAsync(sprint);

        await _messageBroker.PublishAsync(new StartedSprint(sprint.Id));
    }
}