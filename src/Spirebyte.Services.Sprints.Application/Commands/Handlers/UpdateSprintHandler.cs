﻿using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Sprints.Application.Events;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Application.Services.Interfaces;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Commands.Handlers;

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

    public async Task HandleAsync(UpdateSprint command)
    {
        var sprint = await _sprintRepository.GetAsync(command.Id);
        if (sprint is null) throw new SprintNotFoundException(command.Id);

        var newSprint = new Sprint(sprint.Id, command.Title, command.Description, sprint.ProjectId, sprint.IssueIds,
            sprint.CreatedAt, sprint.StartedAt, command.StartDate, command.EndDate, sprint.EndedAt);
        await _sprintRepository.UpdateAsync(newSprint);

        await _messageBroker.PublishAsync(new SprintUpdated(sprint.Id));
    }
}