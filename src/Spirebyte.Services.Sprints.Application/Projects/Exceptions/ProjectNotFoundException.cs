﻿using Spirebyte.Services.Sprints.Application.Exceptions.Base;

namespace Spirebyte.Services.Sprints.Application.Projects.Exceptions;

public class ProjectNotFoundException : AppException
{
    public ProjectNotFoundException(string projectId) : base($"Project with Id: '{projectId}' was not found.")
    {
        ProjectId = projectId;
    }

    public override string Code { get; } = "project_not_found";
    public string ProjectId { get; }
}