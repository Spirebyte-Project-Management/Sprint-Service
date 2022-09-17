using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Sprints.Application.Sprints.Events;
using Spirebyte.Services.Sprints.Application.Sprints.Exceptions;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Sprints.Commands.Handlers;

internal sealed class DeleteSprintHandler : ICommandHandler<DeleteSprint>
{
    private readonly IMessageBroker _messageBroker;
    private readonly ISprintRepository _sprintRepository;

    public DeleteSprintHandler(ISprintRepository sprintRepository,
        IMessageBroker messageBroker)
    {
        _sprintRepository = sprintRepository;
        _messageBroker = messageBroker;
    }

    public async Task HandleAsync(DeleteSprint command, CancellationToken cancellationToken = default)
    {
        if (!await _sprintRepository.ExistsAsync(command.Id)) throw new SprintNotFoundException(command.Id);

        var sprint = await _sprintRepository.GetAsync(command.Id);

        await _sprintRepository.DeleteAsync(sprint.Id);

        await _messageBroker.SendAsync(new SprintDeleted(sprint), cancellationToken);
    }
}