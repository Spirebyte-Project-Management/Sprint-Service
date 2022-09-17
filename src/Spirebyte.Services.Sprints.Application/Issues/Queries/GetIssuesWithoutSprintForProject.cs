using Spirebyte.Framework.Shared.Abstractions;

namespace Spirebyte.Services.Sprints.Application.Issues.Queries;

public class GetIssuesWithoutSprintForProject : IQuery<string[]>
{
    public GetIssuesWithoutSprintForProject(string projectId)
    {
        ProjectId = projectId;
    }

    public string ProjectId { get; set; }
}