using Spirebyte.Services.Sprints.Application.Exceptions.Base;
using System;

namespace Spirebyte.Services.Sprints.Application.Exceptions
{
    public class ProjectNotFoundException : AppException
    {
        public override string Code { get; } = "project_not_found";
        public string Key { get; }
        public Guid ProjectId { get; }

        public ProjectNotFoundException(string key) : base($"Project with Key: '{key}' was not found.")
        {
            Key = key;
        }
        public ProjectNotFoundException(Guid projectId) : base($"Project with Id: '{projectId}' was not found.")
        {
            ProjectId = projectId;
        }
    }
}
