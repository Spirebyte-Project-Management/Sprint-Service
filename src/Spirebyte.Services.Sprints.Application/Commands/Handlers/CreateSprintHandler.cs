using Convey.CQRS.Commands;
using Spirebyte.Services.Sprints.Application.Events;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Application.Services.Interfaces;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;
using System;
using System.Threading.Tasks;

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
            if (string.IsNullOrEmpty(command.ProjectKey) && command.ProjectId != Guid.NewGuid())
            {
                command.ProjectKey = await _projectRepository.GetKeyAsync(command.ProjectId);
            }
            if (!(await _projectRepository.ExistsAsync(command.ProjectKey)))
            {
                throw new ProjectNotFoundException(command.ProjectKey);
            }

            var sprintCount = await _sprintRepository.GetSprintCountOfProjectAsync(command.ProjectId);
            var sprintKey = $"{command.ProjectKey}-Sprint-{sprintCount + 1}";

            var sprint = new Sprint(command.SprintId, sprintKey, command.Title, command.Description, command.ProjectId, null, command.CreatedAt, DateTime.MinValue, command.StartDate, command.EndDate, DateTime.MinValue);
            await _sprintRepository.AddAsync(sprint);
            await _messageBroker.PublishAsync(new SprintCreated(sprint.Id));
        }
    }
}
