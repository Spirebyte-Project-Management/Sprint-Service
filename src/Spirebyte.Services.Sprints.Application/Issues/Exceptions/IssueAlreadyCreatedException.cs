using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Sprints.Application.Issues.Exceptions;

public class IssueAlreadyCreatedException : AppException
{
    public IssueAlreadyCreatedException(string issueId)
        : base($"Issue with id: {issueId} was already created.")
    {
        IssueId = issueId;
    }

    public string Code { get; } = "issue_already_created";
    public string IssueId { get; }
}