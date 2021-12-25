using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Sprints.Application.Events;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Application.Services.Interfaces;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Commands.Handlers;

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

    public async Task HandleAsync(DeleteSprint command)
    {
        if (!await _sprintRepository.ExistsAsync(command.Id)) throw new SprintNotFoundException(command.Id);

        await _sprintRepository.DeleteAsync(command.Id);

        await _messageBroker.PublishAsync(new SprintDeleted(command.Id));
    }
}