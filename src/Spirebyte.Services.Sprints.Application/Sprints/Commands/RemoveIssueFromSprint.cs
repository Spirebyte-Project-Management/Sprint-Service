using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Sprints.Application.Sprints.Commands;

[Message("sprints", "remove_issue_from_sprint", "sprints.remove_issue_from_sprint")]
public class RemoveIssueFromSprint : ICommand
{
    public RemoveIssueFromSprint(string sprintId, string issueId)
    {
        SprintId = sprintId;
        IssueId = issueId;
    }

    public string SprintId { get; set; }
    public string IssueId { get; set; }
}