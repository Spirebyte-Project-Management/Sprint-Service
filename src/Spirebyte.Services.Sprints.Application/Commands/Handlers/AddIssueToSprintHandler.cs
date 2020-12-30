using Convey.CQRS.Commands;
using Spirebyte.Services.Sprints.Application.Events;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Application.Services.Interfaces;
using Spirebyte.Services.Sprints.Core.Repositories;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Application.Commands.Handlers
{
    // Simple wrapper
    internal sealed class AddIssueToSprintHandler : ICommandHandler<AddIssueToSprint>
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IMessageBroker _messageBroker;

        public AddIssueToSprintHandler(ISprintRepository sprintRepository,
            IIssueRepository issueRepository,
            IMessageBroker messageBroker)
        {
            _sprintRepository = sprintRepository;
            _issueRepository = issueRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(AddIssueToSprint command)
        {
            if (!(await _sprintRepository.ExistsAsync(command.SprintId)))
            {
                throw new SprintNotFoundException(command.SprintId);
            }

            if (!(await _issueRepository.ExistsAsync(command.IssueId)))
            {
                throw new IssueNotFoundException(command.IssueId);
            }

            var sprint = await _sprintRepository.GetAsync(command.SprintId);
            var issue = await _issueRepository.GetAsync(command.IssueId);
            issue.AddToSprint(sprint.Id);

            await _issueRepository.UpdateAsync(issue);

            await _messageBroker.PublishAsync(new AddedIssueToSprint(sprint.Id, issue.Id));

        }
    }
}
