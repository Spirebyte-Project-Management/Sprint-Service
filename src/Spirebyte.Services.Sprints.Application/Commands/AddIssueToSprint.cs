using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Sprints.Application.Commands
{
    [Contract]
    public class AddIssueToSprint : ICommand
    {
        public string SprintKey { get; set; }
        public string IssueKey { get; set; }

        public AddIssueToSprint(string sprintKey, string issueKey)
        {
            SprintKey = sprintKey;
            IssueKey = issueKey;
        }
    }
}
