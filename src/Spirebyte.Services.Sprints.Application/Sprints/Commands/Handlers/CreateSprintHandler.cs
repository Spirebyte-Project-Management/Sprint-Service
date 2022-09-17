using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Sprints.Application.Projects.Exceptions;
using Spirebyte.Services.Sprints.Application.Sprints.Events;
using Spirebyte.Services.Sprints.Application.Sprints.Services.Interfaces;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Sprints.Commands.Handlers;

internal sealed class CreateSprintHandler : ICommandHandler<CreateSprint>
{
    private readonly IMessageBroker _messageBroker;
    private readonly IProjectRepository _projectRepository;
    private readonly ISprintRepository _sprintRepository;
    private readonly ISprintRequestStorage _sprintRequestStorage;

    public CreateSprintHandler(IProjectRepository projectRepository, ISprintRepository sprintRepository,
        IMessageBroker messageBroker, ISprintRequestStorage sprintRequestStorage)
    {
        _projectRepository = projectRepository;
        _sprintRepository = sprintRepository;
        _messageBroker = messageBroker;
        _sprintRequestStorage = sprintRequestStorage;
    }

    public async Task HandleAsync(CreateSprint command, CancellationToken cancellationToken = default)
    {
        if (!await _projectRepository.ExistsAsync(command.ProjectId))
            throw new ProjectNotFoundException(command.ProjectId);

        var sprintCount = await _sprintRepository.GetSprintCountOfProjectAsync(command.ProjectId);
        var sprintId = $"{command.ProjectId}-Sprint-{sprintCount + 1}";

        var sprint = new Sprint(sprintId, command.Title, command.Description, command.ProjectId, new List<string>(),
            command.CreatedAt, DateTime.MinValue, command.StartDate, command.EndDate, DateTime.MinValue,
            new List<Change>(), 0, 0);

        await _sprintRepository.AddAsync(sprint);
        await _messageBroker.SendAsync(new SprintCreated(sprint), cancellationToken);

        _sprintRequestStorage.SetSprint(command.ReferenceId, sprint);
    }
}