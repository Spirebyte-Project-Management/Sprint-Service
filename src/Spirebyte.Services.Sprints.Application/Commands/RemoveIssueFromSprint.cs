using Convey.CQRS.Commands;

namespace Spirebyte.Services.Sprints.Application.Commands
{
    [Contract]
    public class RemoveIssueFromSprint : ICommand
    {
        public string SprintId { get; set; }
        public string IssueId { get; set; }

        public RemoveIssueFromSprint(string sprintId, string issueId)
        {
            SprintId = sprintId;
            IssueId = issueId;
        }
    }
}
