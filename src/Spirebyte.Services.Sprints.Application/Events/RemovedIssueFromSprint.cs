using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Events
{
    [Contract]
    public class RemovedIssueFromSprint : IEvent
    {
        public string SprintId { get; }
        public string IssueId { get; }

        public RemovedIssueFromSprint(string sprintId, string issueId)
        {
            SprintId = sprintId;
            IssueId = issueId;
        }
    }
}
