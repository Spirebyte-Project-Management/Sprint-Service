using Spirebyte.Services.Sprints.Core.Exceptions.Base;

namespace Spirebyte.Services.Sprints.Core.Exceptions
{
    public class InvalidProjectIdException : DomainException
    {
        public string ProjectId { get; }
        public override string Code { get; } = "invalid_project_id";

        public InvalidProjectIdException(string projectId) : base($"Invalid project id: {projectId}.")
        {
            ProjectId = projectId;
        }
    }
}
