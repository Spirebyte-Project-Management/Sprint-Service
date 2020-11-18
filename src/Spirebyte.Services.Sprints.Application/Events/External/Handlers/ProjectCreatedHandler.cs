using System.Threading.Tasks;
using Convey.CQRS.Events;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Events.External.Handlers
{
    public class ProjectCreatedHandler : IEventHandler<ProjectCreated>
    {
        private readonly IProjectRepository _projectRepository;


        public ProjectCreatedHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task HandleAsync(ProjectCreated @event)
        {
            if (await _projectRepository.ExistsAsync(@event.ProjectId))
            {
                throw new ProjectAlreadyCreatedException(@event.ProjectId);
            }

            var project = new Project(@event.ProjectId, @event.Key);
            await _projectRepository.AddAsync(project);
        }
    }
}
