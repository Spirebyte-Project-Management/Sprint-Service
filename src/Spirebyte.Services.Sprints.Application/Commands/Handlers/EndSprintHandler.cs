using Convey.CQRS.Commands;
using Spirebyte.Services.Sprints.Application.Events;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Application.Services.Interfaces;
using Spirebyte.Services.Sprints.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Application.Commands.Handlers
{
    internal sealed class EndSprintHandler : ICommandHandler<EndSprint>
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IMessageBroker _messageBroker;

        public EndSprintHandler(ISprintRepository sprintRepository,
            IMessageBroker messageBroker)
        {
            _sprintRepository = sprintRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(EndSprint command)
        {
            if (!(await _sprintRepository.ExistsAsync(command.Id)))
            {
                throw new SprintNotFoundException(command.Id);
            }

            var sprint = await _sprintRepository.GetAsync(command.Id);

            if (sprint.StartedAt == DateTime.MinValue)
            {
                throw new SprintNotStartedException(sprint.Id);
            }

            sprint.End();
            await _sprintRepository.UpdateAsync(sprint);

            await _messageBroker.PublishAsync(new EndedSprint(sprint.Id));

        }
    }
}
