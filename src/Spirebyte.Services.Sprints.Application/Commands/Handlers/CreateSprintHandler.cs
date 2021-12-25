using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Sprints.Application.Events;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Application.Services.Interfaces;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Commands.Handlers;

// Simple wrapper
internal sealed class CreateSprintHandler : ICommandHandler<CreateSprint>
{
    private readonly IMessageBroker _messageBroker;
    private readonly IProjectRepository _projectRepository;
    private readonly ISprintRepository _sprintRepository;

    public CreateSprintHandler(IProjectRepository projectRepository, ISprintRepository sprintRepository,
        IMessageBroker messageBroker)
    {
        _projectRepository = projectRepository;
        _sprintRepository = sprintRepository;
        _messageBroker = messageBroker;
    }

    public async Task HandleAsync(CreateSprint command)
    {
        if (!await _projectRepository.ExistsAsync(command.ProjectId))
            throw new ProjectNotFoundException(command.ProjectId);

        var sprintCount = await _sprintRepository.GetSprintCountOfProjectAsync(command.ProjectId);
        var sprintId = $"{command.ProjectId}-Sprint-{sprintCount + 1}";

        var sprint = new Sprint(sprintId, command.Title, command.Description, command.ProjectId, new string[] { },
            command.CreatedAt, DateTime.MinValue, command.StartDate, command.EndDate, DateTime.MinValue);
        await _sprintRepository.AddAsync(sprint);
        await _messageBroker.PublishAsync(new SprintCreated(sprint.Id));
    }
}