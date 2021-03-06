using Spirebyte.Services.Sprints.Application.Exceptions.Base;

namespace Spirebyte.Services.Sprints.Application.Exceptions
{
    public class ProjectAlreadyCreatedException : AppException
    {
        public override string Code { get; } = "project_already_created";
        public string ProjectId { get; }

        public ProjectAlreadyCreatedException(string projectId)
            : base($"Project with id: {projectId} was already created.")
        {
            ProjectId = projectId;
        }
    }
}
