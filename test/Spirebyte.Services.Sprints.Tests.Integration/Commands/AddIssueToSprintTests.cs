using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.API;
using Spirebyte.Services.Sprints.Application.Commands;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Entities.Base;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers;
using Spirebyte.Services.Sprints.Tests.Shared.Factories;
using Spirebyte.Services.Sprints.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Commands
{
    [Collection("Spirebyte collection")]
    public class AddIssueToSprintTests : IDisposable
    {
        public AddIssueToSprintTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _sprintRepository = factory.Services.GetRequiredService<IEfRepository<SprintsDbContext, SprintTable, Guid>>();
            _projectRepository = factory.Services.GetRequiredService<IEfRepository<SprintsDbContext, ProjectTable, Guid>>();
            _issueRepository = factory.Services.GetRequiredService<IEfRepository<SprintsDbContext, IssueTable, Guid>>();
            _dbContext = factory.Services.GetRequiredService<SprintsDbContext>();
            factory.Server.AllowSynchronousIO = true;
            _commandHandler = factory.Services.GetRequiredService<ICommandHandler<AddIssueToSprint>>();
        }

        public async void Dispose()
        {
            _dbContext.Sprints.RemoveRange(_dbContext.Sprints);
            _dbContext.Projects.RemoveRange(_dbContext.Projects);
            _dbContext.Issues.RemoveRange(_dbContext.Issues);
            await _dbContext.SaveChangesAsync();
        }

        private const string Exchange = "sprints";
        private readonly IEfRepository<SprintsDbContext, SprintTable, Guid> _sprintRepository;
        private readonly IEfRepository<SprintsDbContext, ProjectTable, Guid> _projectRepository;
        private readonly IEfRepository<SprintsDbContext, IssueTable, Guid> _issueRepository;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly SprintsDbContext _dbContext;
        private readonly ICommandHandler<AddIssueToSprint> _commandHandler;


        [Fact]
        public async Task add_issue_to_sprint_command_should_ass_issue_to_sprint()
        {
            var projectId = Guid.NewGuid();
            var projectKey = "project-key";

            var project = new Project(projectId, projectKey);
            await _projectRepository.AddAsync(project.AsDocument());

            var sprintId = Guid.NewGuid();
            var sprintKey = "key-sprint-1";
            var title = "Title";
            var description = "description";
            var createdAt = DateTime.Now;
            var startedAt = DateTime.MinValue;
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MaxValue;
            var endedAt = DateTime.MaxValue;

            var sprint = new Sprint(sprintId, sprintKey, title, description, projectId, null, createdAt, startedAt, startDate, endDate, endedAt);
            await _sprintRepository.AddAsync(sprint.AsDocument());

            var issueId = Guid.NewGuid();
            var issuekey = "key";

            var issue = new Issue(issueId, issuekey, projectId, null);
            await _issueRepository.AddAsync(issue.AsDocument());


            var command = new AddIssueToSprint(sprintKey, issuekey);

            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().NotThrow();


            var removedIssue = await _issueRepository.GetAsync(issueId);

            removedIssue.Should().NotBeNull();
            removedIssue.SprintId.Should().Be(sprintId);
        }

        [Fact]
        public async void add_issue_to_sprint_command_fails_when_sprint_does_not_exist()
        {
            var projectId = Guid.NewGuid();
            var projectKey = "project-key";

            var project = new Project(projectId, projectKey);
            await _projectRepository.AddAsync(project.AsDocument());

            var sprintId = Guid.NewGuid();
            var sprintKey = "key-sprint-1";

            var issueId = Guid.NewGuid();
            var issuekey = "key";

            var issue = new Issue(issueId, issuekey, projectId, null);
            await _issueRepository.AddAsync(issue.AsDocument());


            var command = new AddIssueToSprint(sprintKey, issuekey);


            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().Throw<SprintNotFoundException>();
        }

        [Fact]
        public async void add_issue_to_sprint_command_fails_when_issue_does_not_exist()
        {
            var projectId = Guid.NewGuid();
            var projectKey = "project-key";

            var project = new Project(projectId, projectKey);
            await _projectRepository.AddAsync(project.AsDocument());

            var sprintId = Guid.NewGuid();
            var sprintKey = "key-sprint-1";
            var title = "Title";
            var description = "description";
            var createdAt = DateTime.Now;
            var startedAt = DateTime.MinValue;
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MaxValue;
            var endedAt = DateTime.MaxValue;

            var sprint = new Sprint(sprintId, sprintKey, title, description, projectId, null, createdAt, startedAt, startDate, endDate, endedAt);
            await _sprintRepository.AddAsync(sprint.AsDocument());

            var issuekey = "key";


            var command = new AddIssueToSprint(sprintKey, issuekey);


            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().Throw<IssueNotFoundException>();
        }
    }
}
