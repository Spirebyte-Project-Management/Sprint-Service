using Convey.CQRS.Queries;

namespace Spirebyte.Services.Sprints.Application.Queries;

public class GetIssuesWithoutSprintForProject : IQuery<string[]>
{
    public GetIssuesWithoutSprintForProject(string projectId)
    {
        ProjectId = projectId;
    }

    public string ProjectId { get; set; }
}