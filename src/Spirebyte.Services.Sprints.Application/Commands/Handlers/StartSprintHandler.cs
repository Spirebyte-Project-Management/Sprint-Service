using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Sprints.Application.Events;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Application.Services.Interfaces;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Commands.Handlers
{
    internal sealed class StartSprintHandler : ICommandHandler<StartSprint>
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IMessageBroker _messageBroker;

        public StartSprintHandler(ISprintRepository sprintRepository,
            IMessageBroker messageBroker)
        {
            _sprintRepository = sprintRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(StartSprint command)
        {
            if (!(await _sprintRepository.ExistsAsync(command.Id)))
            {
                throw new SprintNotFoundException(command.Id);
            }

            var sprint = await _sprintRepository.GetAsync(command.Id);
            sprint.Start();
            await _sprintRepository.UpdateAsync(sprint);

            await _messageBroker.PublishAsync(new StartedSprint(sprint.Id));

        }
    }
}
