using Convey.CQRS.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Sprints.API;
using Spirebyte.Services.Sprints.Application.Commands;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Sprints.Tests.Shared.Factories;
using Spirebyte.Services.Sprints.Tests.Shared.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Commands
{
    [Collection("Spirebyte collection")]
    public class AddIssueToSprintTests : IDisposable
    {
        public AddIssueToSprintTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _sprintMongoDbFixture = new MongoDbFixture<SprintDocument, string>("sprints");
            _projectMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
            _issueMongoDbFixture = new MongoDbFixture<IssueDocument, string>("issues");
            factory.Server.AllowSynchronousIO = true;
            _commandHandler = factory.Services.GetRequiredService<ICommandHandler<AddIssueToSprint>>();
        }

        public async void Dispose()
        {
            _sprintMongoDbFixture.Dispose();
            _projectMongoDbFixture.Dispose();
            _issueMongoDbFixture.Dispose();
        }

        private const string Exchange = "sprints";
        private readonly MongoDbFixture<SprintDocument, string> _sprintMongoDbFixture;
        private readonly MongoDbFixture<ProjectDocument, string> _projectMongoDbFixture;
        private readonly MongoDbFixture<IssueDocument, string> _issueMongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly ICommandHandler<AddIssueToSprint> _commandHandler;


        [Fact]
        public async Task add_issue_to_sprint_command_should_ass_issue_to_sprint()
        {
            var projectId = "projectKey" + Guid.NewGuid();

            var project = new Project(projectId);
            await _projectMongoDbFixture.InsertAsync(project.AsDocument());

            var sprintId = "sprintKey" + Guid.NewGuid();
            var title = "Title";
            var description = "description";
            var createdAt = DateTime.Now;
            var startedAt = DateTime.MinValue;
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MaxValue;
            var endedAt = DateTime.MaxValue;

            var sprint = new Sprint(sprintId, title, description, projectId, null, createdAt, startedAt, startDate, endDate, endedAt);
            await _sprintMongoDbFixture.InsertAsync(sprint.AsDocument());

            var issueId = "issueKey" + Guid.NewGuid();

            var issue = new Issue(issueId, projectId, null);
            await _issueMongoDbFixture.InsertAsync(issue.AsDocument());


            var command = new AddIssueToSprint(sprintId, issueId);

            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().NotThrow();


            var removedIssue = await _issueMongoDbFixture.GetAsync(issueId);

            removedIssue.Should().NotBeNull();
            removedIssue.SprintId.Should().Be(sprintId);
        }

        [Fact]
        public async void add_issue_to_sprint_command_fails_when_sprint_does_not_exist()
        {
            var projectId = "projectKey" + Guid.NewGuid();

            var project = new Project(projectId);
            await _projectMongoDbFixture.InsertAsync(project.AsDocument());

            var sprintId = "sprintKey" + Guid.NewGuid();

            var issueId = "issueKey" + Guid.NewGuid();

            var issue = new Issue(issueId, projectId, null);
            await _issueMongoDbFixture.InsertAsync(issue.AsDocument());


            var command = new AddIssueToSprint(sprintId, issueId);


            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().Throw<SprintNotFoundException>();
        }

        [Fact]
        public async void add_issue_to_sprint_command_fails_when_issue_does_not_exist()
        {
            var projectId = "projectId" + Guid.NewGuid();

            var project = new Project(projectId);
            await _projectMongoDbFixture.InsertAsync(project.AsDocument());

            var sprintId = "sprintKey" + Guid.NewGuid();
            var title = "Title";
            var description = "description";
            var createdAt = DateTime.Now;
            var startedAt = DateTime.MinValue;
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MaxValue;
            var endedAt = DateTime.MaxValue;

            var sprint = new Sprint(sprintId, title, description, projectId, null, createdAt, startedAt, startDate, endDate, endedAt);
            await _sprintMongoDbFixture.InsertAsync(sprint.AsDocument());

            var issueId = "issueKey" + Guid.NewGuid();


            var command = new AddIssueToSprint(sprintId, issueId);


            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().Throw<IssueNotFoundException>();
        }
    }
}
