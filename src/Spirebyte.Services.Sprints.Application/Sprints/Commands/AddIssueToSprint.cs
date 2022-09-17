using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Sprints.Application.Sprints.Commands;

[Message("sprints", "add_issue_to_sprint", "sprints.add_issue_to_sprint")]
public class AddIssueToSprint : ICommand
{
    public AddIssueToSprint(string sprintId, string issueId)
    {
        SprintId = sprintId;
        IssueId = issueId;
    }

    public string SprintId { get; set; }
    public string IssueId { get; set; }
}