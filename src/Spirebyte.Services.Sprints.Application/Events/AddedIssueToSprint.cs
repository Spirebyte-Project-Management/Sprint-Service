using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Events
{
    [Contract]
    public class AddedIssueToSprint : IEvent
    {
        public string SprintId { get; }
        public string IssueId { get; }

        public AddedIssueToSprint(string sprintId, string issueId)
        {
            SprintId = sprintId;
            IssueId = issueId;
        }
    }
}
