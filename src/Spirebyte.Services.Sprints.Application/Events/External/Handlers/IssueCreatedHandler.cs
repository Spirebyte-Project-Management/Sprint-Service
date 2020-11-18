using System.Threading.Tasks;
using Convey.CQRS.Events;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;

namespace Spirebyte.Services.Sprints.Application.Events.External.Handlers
{
    public class SignedUpHandler : IEventHandler<IssueCreated>
    {
        private readonly IIssueRepository _issueRepository;

        public SignedUpHandler(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        public async Task HandleAsync(IssueCreated @event)
        {
            if (await _issueRepository.ExistsAsync(@event.IssueId))
            {
                throw new IssueAlreadyCreatedException(@event.IssueId);
            }

            var issue = new Issue(@event.IssueId, @event.IssueKey);
            await _issueRepository.AddAsync(issue);
        }
    }
}
