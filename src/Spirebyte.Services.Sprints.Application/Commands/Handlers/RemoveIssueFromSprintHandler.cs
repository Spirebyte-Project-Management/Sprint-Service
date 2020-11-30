using Convey.CQRS.Commands;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Core.Repositories;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Application.Commands.Handlers
{
    // Simple wrapper
    internal sealed class RemoveIssueFromSprintHandler : ICommandHandler<RemoveIssueFromSprint>
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IIssueRepository _issueRepository;

        public RemoveIssueFromSprintHandler(ISprintRepository sprintRepository,
            IIssueRepository issueRepository)
        {
            _sprintRepository = sprintRepository;
            _issueRepository = issueRepository;
        }

        public async Task HandleAsync(RemoveIssueFromSprint command)
        {
            if (!(await _sprintRepository.ExistsAsync(command.SprintKey)))
            {
                throw new SprintNotFoundException(command.SprintKey);
            }

            if (!(await _issueRepository.ExistsAsync(command.IssueKey)))
            {
                throw new IssueNotFoundException(command.IssueKey);
            }

            var issue = await _issueRepository.GetAsync(command.IssueKey);
            issue.RemoveFromSprint();

            await _issueRepository.UpdateAsync(issue);
        }
    }
}
