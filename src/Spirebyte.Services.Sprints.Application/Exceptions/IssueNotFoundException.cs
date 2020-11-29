using System;
using Spirebyte.Services.Sprints.Application.Exceptions.Base;

namespace Spirebyte.Services.Sprints.Application.Exceptions
{
    public class IssueNotFoundException : AppException
    {
        public override string Code { get; } = "issue_not_found";
        public string SprintKey { get; }

        public IssueNotFoundException(string issueKey) : base($"Issue with key: '{issueKey}' was not found.")
        {
            SprintKey = issueKey;
        }
    }
}
