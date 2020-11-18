using System;
using Spirebyte.Services.Sprints.Core.Exceptions.Base;

namespace Spirebyte.Services.Sprints.Core.Exceptions
{
    public class InvalidProjectIdException : DomainException
    {
        public Guid ProjectId { get; }
        public override string Code { get; } = "invalid_project_id";

        public InvalidProjectIdException(Guid projectId) : base($"Invalid project id: {projectId}.")
        {
            ProjectId = projectId;
        }
    }
}
