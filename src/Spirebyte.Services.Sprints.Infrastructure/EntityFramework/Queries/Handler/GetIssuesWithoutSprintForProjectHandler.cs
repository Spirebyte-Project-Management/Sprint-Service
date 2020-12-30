using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.Application.Queries;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using System.Linq;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Queries.Handler
{
    internal sealed class GetIssuesWithoutSprintForProjectHandler : IQueryHandler<GetIssuesWithoutSprintForProject, string[]>
    {
        private readonly IEfRepository<SprintsDbContext, IssueTable, string> _issueRepository;
        private readonly IEfRepository<SprintsDbContext, ProjectTable, string> _projectRepository;

        public GetIssuesWithoutSprintForProjectHandler(IEfRepository<SprintsDbContext, IssueTable, string> issueRepository,
            IEfRepository<SprintsDbContext, ProjectTable, string> projectRepository)
        {
            _issueRepository = issueRepository;
            _projectRepository = projectRepository;
        }

        public async Task<string[]> HandleAsync(GetIssuesWithoutSprintForProject query)
        {
            var project = await _projectRepository.GetAsync(query.ProjectId);

            var issueIds = await _issueRepository.Collection.Where(i => i.ProjectId == project.Id && (i.SprintId == null || i.SprintId == string.Empty)).Select(i => i.Id).ToArrayAsync();

            return issueIds;
        }
    }
}
