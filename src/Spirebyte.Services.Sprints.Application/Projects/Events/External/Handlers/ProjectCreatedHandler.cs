using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
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
        if (await _projectRepository.ExistsAsync(@event.ProjectId))
            throw new ProjectAlreadyCreatedException(@event.ProjectId);

        var project = new Project(@event.ProjectId);
        await _projectRepository.AddAsync(project);
    }
}