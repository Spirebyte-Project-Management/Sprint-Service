using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.Application.Queries;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Queries.Handler
{
    internal sealed class GetIssuesWithoutSprintForProjectHandler : IQueryHandler<GetIssuesWithoutSprintForProject, Guid[]>
    {
        private readonly IEfRepository<SprintsDbContext, IssueTable, Guid> _issueRepository;
        private readonly IEfRepository<SprintsDbContext, ProjectTable, Guid> _projectRepository;

        public GetIssuesWithoutSprintForProjectHandler(IEfRepository<SprintsDbContext, IssueTable, Guid> issueRepository,
            IEfRepository<SprintsDbContext, ProjectTable, Guid> projectRepository)
        {
            _issueRepository = issueRepository;
            _projectRepository = projectRepository;
        }

        public async Task<Guid[]> HandleAsync(GetIssuesWithoutSprintForProject query)
        {
            var project = await _projectRepository.GetAsync(p => p.Key == query.ProjectKey);

            var issueIds = await _issueRepository.Collection.Where(i => i.ProjectId == project.Id && (i.SprintId == null || i.SprintId == Guid.Empty)).Select(i => i.Id).ToArrayAsync();

            return issueIds;
        }
    }
}
