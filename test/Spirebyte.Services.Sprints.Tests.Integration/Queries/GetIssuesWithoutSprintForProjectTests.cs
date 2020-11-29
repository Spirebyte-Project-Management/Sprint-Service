using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.API;
using Spirebyte.Services.Sprints.Application.DTO;
using Spirebyte.Services.Sprints.Application.Queries;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Entities.Base;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers;
using Spirebyte.Services.Sprints.Tests.Shared.Factories;
using Spirebyte.Services.Sprints.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Queries
{
    [Collection("Spirebyte collection")]
    public class GetIssuesWithoutSprintForProjectTests : IDisposable
    {
        public GetIssuesWithoutSprintForProjectTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _issueRepository = factory.Services.GetRequiredService<IEfRepository<SprintsDbContext, IssueTable, Guid>>();
            _projectRepository = factory.Services.GetRequiredService<IEfRepository<SprintsDbContext, ProjectTable, Guid>>();
            _sprintRepository = factory.Services.GetRequiredService<IEfRepository<SprintsDbContext, SprintTable, Guid>>();
            _dbContext = factory.Services.GetRequiredService<SprintsDbContext>();
            factory.Server.AllowSynchronousIO = true;
            _queryHandler = factory.Services.GetRequiredService<IQueryHandler<GetIssuesWithoutSprintForProject, Guid[]>>();
        }

        public async void Dispose()
        {
            _dbContext.Sprints.RemoveRange(_dbContext.Sprints);
            _dbContext.Projects.RemoveRange(_dbContext.Projects);
            _dbContext.Issues.RemoveRange(_dbContext.Issues);
            await _dbContext.SaveChangesAsync();
        }

        private const string Exchange = "sprints";
        private readonly IEfRepository<SprintsDbContext, IssueTable, Guid> _issueRepository;
        private readonly IEfRepository<SprintsDbContext, ProjectTable, Guid> _projectRepository;
        private readonly IEfRepository<SprintsDbContext, SprintTable, Guid> _sprintRepository;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly SprintsDbContext _dbContext;
        private readonly IQueryHandler<GetIssuesWithoutSprintForProject, Guid[]> _queryHandler;


        [Fact]
        public async Task get_issues_without_sprint_for_project_query_succeeds_when_a_issue_without_sprint_exists()
        {
            var projectId = Guid.NewGuid();
            var projectKey = "project-key";

            var project = new Project(projectId, projectKey);
            await _projectRepository.AddAsync(project.AsDocument());

            var sprintId = Guid.NewGuid();
            var sprintkey = "key-sprint-1";
            var title = "Title";
            var description = "description";
            var createdAt = DateTime.Now;
            var startedAt = DateTime.MinValue;
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MaxValue;
            var endedAt = DateTime.MaxValue;

            var sprint = new Sprint(sprintId, sprintkey, title, description, projectId, null, createdAt, startedAt, startDate, endDate, endedAt);

            await _sprintRepository.AddAsync(sprint.AsDocument());

            var issueId = Guid.NewGuid();
            var issueWithoutSprintId = Guid.NewGuid();
            var issuekey = "key";

            var issue = new Issue(issueId, issuekey, projectId, sprintId);
            var issueWithoutSprint = new Issue(issueWithoutSprintId, issuekey, projectId, null);
            await _issueRepository.AddAsync(issue.AsDocument());
            await _issueRepository.AddAsync(issueWithoutSprint.AsDocument());

            var query = new GetIssuesWithoutSprintForProject(projectKey);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().NotContain(issueId);
            result.Should().Contain(issueWithoutSprintId);
        }

        [Fact]
        public async Task get_issues_without_sprint_for_project_query_should_return_empty_when_no_issues_without_sprint_exist()
        {
            var projectId = Guid.NewGuid();
            var projectKey = "project-key";

            var project = new Project(projectId, projectKey);
            await _projectRepository.AddAsync(project.AsDocument());

            var sprintId = Guid.NewGuid();
            var sprintkey = "key-sprint-1";
            var title = "Title";
            var description = "description";
            var createdAt = DateTime.Now;
            var startedAt = DateTime.MinValue;
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MaxValue;
            var endedAt = DateTime.MaxValue;

            var sprint = new Sprint(sprintId, sprintkey, title, description, projectId, null, createdAt, startedAt, startDate, endDate, endedAt);
            await _sprintRepository.AddAsync(sprint.AsDocument());

            var issueId = Guid.NewGuid();
            var issue2Id = Guid.NewGuid();
            var issuekey = "key";

            var issue = new Issue(issueId, issuekey, projectId, sprintId);
            var issue2 = new Issue(issue2Id, issuekey, projectId, sprintId);
            await _issueRepository.AddAsync(issue.AsDocument());
            await _issueRepository.AddAsync(issue2.AsDocument());

            var query = new GetIssuesWithoutSprintForProject(projectKey);
            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().BeEmpty();
        }
    }
}
