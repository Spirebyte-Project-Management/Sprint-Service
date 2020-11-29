using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Queries;

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
