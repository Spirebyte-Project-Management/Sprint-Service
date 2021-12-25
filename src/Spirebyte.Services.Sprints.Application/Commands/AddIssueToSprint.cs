using Convey.CQRS.Commands;

namespace Spirebyte.Services.Sprints.Application.Commands;

[Contract]
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