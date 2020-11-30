using Convey.CQRS.Commands;

namespace Spirebyte.Services.Sprints.Application.Commands
{
    [Contract]
    public class RemoveIssueFromSprint : ICommand
    {
        public string SprintKey { get; set; }
        public string IssueKey { get; set; }

        public RemoveIssueFromSprint(string sprintKey, string issueKey)
        {
            SprintKey = sprintKey;
            IssueKey = issueKey;
        }
    }
}
