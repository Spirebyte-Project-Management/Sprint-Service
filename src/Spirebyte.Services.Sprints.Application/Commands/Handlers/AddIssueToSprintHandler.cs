using Convey.CQRS.Commands;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Core.Repositories;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Application.Commands.Handlers
{
    // Simple wrapper
    internal sealed class AddIssueToSprintHandler : ICommandHandler<AddIssueToSprint>
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IIssueRepository _issueRepository;

        public AddIssueToSprintHandler(ISprintRepository sprintRepository,
            IIssueRepository issueRepository)
        {
            _sprintRepository = sprintRepository;
            _issueRepository = issueRepository;
        }

        public async Task HandleAsync(AddIssueToSprint command)
        {
            if (!(await _sprintRepository.ExistsAsync(command.SprintKey)))
            {
                throw new SprintNotFoundException(command.SprintKey);
            }

            if (!(await _issueRepository.ExistsAsync(command.IssueKey)))
            {
                throw new IssueNotFoundException(command.IssueKey);
            }

            var sprint = await _sprintRepository.GetAsync(command.SprintKey);
            var issue = await _issueRepository.GetAsync(command.IssueKey);
            issue.AddToSprint(sprint.Id);

            await _issueRepository.UpdateAsync(issue);
        }
    }
}
