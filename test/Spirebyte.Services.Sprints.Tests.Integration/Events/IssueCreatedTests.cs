using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.API;
using Spirebyte.Services.Sprints.Application.Events.External;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers;
using Spirebyte.Services.Sprints.Tests.Shared.Factories;
using Spirebyte.Services.Sprints.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Events
{
    [Collection("Spirebyte collection")]
    public class IssueCreatedTests : IDisposable
    {
        public IssueCreatedTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _issueRepository = factory.Services.GetRequiredService<IEfRepository<SprintsDbContext, IssueTable, Guid>>();
            _projectRepository = factory.Services.GetRequiredService<IEfRepository<SprintsDbContext, ProjectTable, Guid>>();
            _dbContext = factory.Services.GetRequiredService<SprintsDbContext>();
            factory.Server.AllowSynchronousIO = true;
            _eventHandler = factory.Services.GetRequiredService<IEventHandler<IssueCreated>>();
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
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly SprintsDbContext _dbContext;
        private readonly IEventHandler<IssueCreated> _eventHandler;


        [Fact]
        public async Task issue_created_event_should_add_issue_with_given_data_to_database()
        {
            var issueId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var key = "key";
            var projectKey = "project-key";

            var project = new Project(projectId, projectKey);
            await _projectRepository.AddAsync(project.AsDocument());

            var externalEvent = new IssueCreated(issueId, key, projectId);

            // Check if exception is thrown

            _eventHandler
                .Awaiting(c => c.HandleAsync(externalEvent))
                .Should().NotThrow();


            var issue = await _issueRepository.GetAsync(issueId);

            issue.Should().NotBeNull();
            issue.Id.Should().Be(issueId);
            issue.Key.Should().Be(key);
            issue.ProjectId.Should().Be(projectId);
            issue.SprintId.Should().BeNull();
        }

        [Fact]
        public async Task issue_created_event_fails_when_issue_with_id_already_exists()
        {
            var issueId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var key = "key";
            var projectKey = "project-key";

            var project = new Project(projectId, projectKey);
            await _projectRepository.AddAsync(project.AsDocument());

            var issue = new Issue(issueId, key, projectId, null);
            await _issueRepository.AddAsync(issue.AsDocument());

            var externalEvent = new IssueCreated(issueId, key, projectId);

            // Check if exception is thrown

            _eventHandler
                .Awaiting(c => c.HandleAsync(externalEvent))
                .Should().Throw<IssueAlreadyCreatedException>();
        }

        [Fact]
        public async Task issue_created_event_fails_when_project_with_id_does_not_exist()
        {
            var issueId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var sprintId = Guid.NewGuid();
            var key = "key";
            var projectKey = "project-key";


            var externalEvent = new IssueCreated(issueId, key, projectId);

            // Check if exception is thrown

            _eventHandler
                .Awaiting(c => c.HandleAsync(externalEvent))
                .Should().Throw<ProjectNotFoundException>();
        }
    }
}
