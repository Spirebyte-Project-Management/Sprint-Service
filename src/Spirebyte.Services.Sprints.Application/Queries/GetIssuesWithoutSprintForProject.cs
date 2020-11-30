using Convey.CQRS.Queries;
using System;

namespace Spirebyte.Services.Sprints.Application.Queries
{
    public class GetIssuesWithoutSprintForProject : IQuery<Guid[]>
    {
        public string ProjectKey { get; set; }

        public GetIssuesWithoutSprintForProject(string projectKey)
        {
            ProjectKey = projectKey;
        }
    }
}
