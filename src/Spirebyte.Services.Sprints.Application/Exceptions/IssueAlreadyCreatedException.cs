using System;
using Spirebyte.Services.Sprints.Application.Exceptions.Base;

namespace Spirebyte.Services.Sprints.Application.Exceptions
{
    public class IssueAlreadyCreatedException : AppException
    {
        public override string Code { get; } = "issue_already_created";
        public Guid IssueId { get; }

        public IssueAlreadyCreatedException(Guid issueId)
            : base($"Issue with id: {issueId} was already created.")
        {
            IssueId = issueId;
        }
    }
}
