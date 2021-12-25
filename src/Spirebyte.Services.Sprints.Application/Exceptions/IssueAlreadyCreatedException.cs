using Spirebyte.Services.Sprints.Application.Exceptions.Base;

namespace Spirebyte.Services.Sprints.Application.Exceptions;

public class IssueAlreadyCreatedException : AppException
{
    public IssueAlreadyCreatedException(string issueId)
        : base($"Issue with id: {issueId} was already created.")
    {
        IssueId = issueId;
    }

    public override string Code { get; } = "issue_already_created";
    public string IssueId { get; }
}