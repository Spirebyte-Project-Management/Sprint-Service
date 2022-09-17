using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Sprints.Application.Issues.Exceptions;

public class IssueNotFoundException : AppException
{
    public IssueNotFoundException(string issueKey) : base($"Issue with key: '{issueKey}' was not found.")
    {
        SprintKey = issueKey;
    }

    public string Code { get; } = "issue_not_found";
    public string SprintKey { get; }
}