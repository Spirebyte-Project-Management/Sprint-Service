using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Events;

[Contract]
public class AddedIssueToSprint : IEvent
{
    public AddedIssueToSprint(string sprintId, string issueId)
    {
        SprintId = sprintId;
        IssueId = issueId;
    }

    public string SprintId { get; }
    public string IssueId { get; }
}