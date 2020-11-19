using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Sprints.Application.Events;
using Spirebyte.Services.Sprints.Application.Events.External;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Application.Services.Interfaces;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Commands.Handlers
{
    // Simple wrapper
    internal sealed class CreateSprintHandler : ICommandHandler<CreateSprint>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ISprintRepository _sprintRepository;
        private readonly IMessageBroker _messageBroker;

        public CreateSprintHandler(IProjectRepository projectRepository, ISprintRepository sprintRepository,
            IMessageBroker messageBroker)
        {
            _projectRepository = projectRepository;
            _sprintRepository = sprintRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(CreateSprint command)
        {
            if (!(await _projectRepository.ExistsAsync(command.ProjectKey)))
            {
                throw new ProjectNotFoundException(command.ProjectKey);
            }

            var projectkey = await _projectRepository.GetKeyAsync(command.ProjectId);
            var sprintCount = await _sprintRepository.GetSprintCountOfProjectAsync(command.ProjectId);
            var sprintKey = $"{projectkey}-Sprint-{sprintCount + 1}";

            var sprint = new Sprint(command.SprintId, sprintKey, command.Title, command.Description, command.ProjectId, command.CreatedAt, DateTime.MinValue, command.StartDate, command.EndDate, DateTime.MinValue);
            await _sprintRepository.AddAsync(sprint);
            await _messageBroker.PublishAsync(new SprintCreated(sprint.Id));
        }
    }
}
