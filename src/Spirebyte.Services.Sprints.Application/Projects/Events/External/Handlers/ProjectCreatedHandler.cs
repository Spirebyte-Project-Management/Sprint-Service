﻿using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Sprints.Application.Projects.Exceptions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Projects.Events.External.Handlers;

internal sealed class ProjectCreatedHandler : IEventHandler<ProjectCreated>
{
    private readonly IProjectRepository _projectRepository;

    public ProjectCreatedHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task HandleAsync(ProjectCreated @event, CancellationToken cancellationToken = default)
    {
        if (await _projectRepository.ExistsAsync(@event.Id))
            throw new ProjectAlreadyCreatedException(@event.Id);

        var project = new Project(@event.Id);
        await _projectRepository.AddAsync(project);
    }
}