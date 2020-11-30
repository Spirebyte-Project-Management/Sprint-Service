using Spirebyte.Services.Sprints.Application.Exceptions.Base;
using System;

namespace Spirebyte.Services.Sprints.Application.Exceptions
{
    public class ProjectAlreadyCreatedException : AppException
    {
        public override string Code { get; } = "project_already_created";
        public Guid ProjectId { get; }

        public ProjectAlreadyCreatedException(Guid projectId)
            : base($"Project with id: {projectId} was already created.")
        {
            ProjectId = projectId;
        }
    }
}
