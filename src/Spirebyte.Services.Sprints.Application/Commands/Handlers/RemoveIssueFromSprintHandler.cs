using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Sprints.Application.Events;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Application.Services.Interfaces;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Commands.Handlers
{
    // Simple wrapper
    internal sealed class RemoveIssueFromSprintHandler : ICommandHandler<RemoveIssueFromSprint>
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IMessageBroker _messageBroker;

        public RemoveIssueFromSprintHandler(ISprintRepository sprintRepository,
            IIssueRepository issueRepository,
            IMessageBroker messageBroker)
        {
            _sprintRepository = sprintRepository;
            _issueRepository = issueRepository;
            _messageBroker = messageBroker;
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
