using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Sprints.API;
using Spirebyte.Services.Sprints.Application.Commands;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Enums;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Sprints.Tests.Shared.Factories;
using Spirebyte.Services.Sprints.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Commands;

[Collection("Spirebyte collection")]
public class RemoveIssueFromSprintTests : IDisposable
{
    private const string Exchange = "sprints";
    private readonly ICommandHandler<RemoveIssueFromSprint> _commandHandler;
    private readonly MongoDbFixture<IssueDocument, string> _issueMongoDbFixture;
    private readonly MongoDbFixture<ProjectDocument, string> _projectMongoDbFixture;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<SprintDocument, string> _sprintMongoDbFixture;

    public RemoveIssueFromSprintTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _sprintMongoDbFixture = new MongoDbFixture<SprintDocument, string>("sprints");
        _projectMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
        _issueMongoDbFixture = new MongoDbFixture<IssueDocument, string>("issues");
        factory.Server.AllowSynchronousIO = true;
        _commandHandler = factory.Services.GetRequiredService<ICommandHandler<RemoveIssueFromSprint>>();
    }

    public async void Dispose()
    {
        _sprintMongoDbFixture.Dispose();
        _projectMongoDbFixture.Dispose();
        _issueMongoDbFixture.Dispose();
    }

    [Fact]
    public async Task remove_issue_from_sprint_command_should_remove_issue_from_sprint()
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

        var sprint = new Sprint(sprintId, title, description, projectId, null, createdAt, startedAt, startDate, endDate,
            endedAt, new List<Change>(), 0, 0);
        await _sprintMongoDbFixture.InsertAsync(sprint.AsDocument());

        var issueId = "issueKey" + Guid.NewGuid();

        var issue = new Issue(issueId, projectId, sprintId, 0, IssueStatus.TODO);
        await _issueMongoDbFixture.InsertAsync(issue.AsDocument());


        var command = new RemoveIssueFromSprint(sprintId, issueId);

        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();


        var removedIssue = await _issueMongoDbFixture.GetAsync(issueId);

        removedIssue.Should().NotBeNull();
        removedIssue.SprintId.Should().BeNull();
    }

    [Fact]
    public async void remove_issue_from_sprint_command_fails_when_sprint_does_not_exist()
    {
        var projectId = "projectKey" + Guid.NewGuid();

        var project = new Project(projectId);
        await _projectMongoDbFixture.InsertAsync(project.AsDocument());

        var sprintId = "sprintKey" + Guid.NewGuid();

        var issueId = "issueKey" + Guid.NewGuid();

        var issue = new Issue(issueId, projectId, null, 0, IssueStatus.TODO);
        await _issueMongoDbFixture.InsertAsync(issue.AsDocument());


        var command = new RemoveIssueFromSprint(sprintId, issueId);


        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<SprintNotFoundException>();
    }

    [Fact]
    public async void remove_issue_from_sprint_command_fails_when_issue_does_not_exist()
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

        var sprint = new Sprint(sprintId, title, description, projectId, null, createdAt, startedAt, startDate, endDate,
            endedAt, new List<Change>(), 0, 0);
        await _sprintMongoDbFixture.InsertAsync(sprint.AsDocument());

        var issueId = "issueKey" + Guid.NewGuid();


        var command = new RemoveIssueFromSprint(sprintId, issueId);


        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<IssueNotFoundException>();
    }
}