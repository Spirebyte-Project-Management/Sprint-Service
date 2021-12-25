using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Sprints.API;
using Spirebyte.Services.Sprints.Application.Events.External;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Sprints.Tests.Shared.Factories;
using Spirebyte.Services.Sprints.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Events;

[Collection("Spirebyte collection")]
public class IssueCreatedTests : IDisposable
{
    private const string Exchange = "sprints";
    private readonly IEventHandler<IssueCreated> _eventHandler;
    private readonly MongoDbFixture<IssueDocument, string> _issueMongoDbFixture;
    private readonly MongoDbFixture<ProjectDocument, string> _projectMongoDbFixture;
    private readonly RabbitMqFixture _rabbitMqFixture;

    public IssueCreatedTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _projectMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
        _issueMongoDbFixture = new MongoDbFixture<IssueDocument, string>("issues");
        factory.Server.AllowSynchronousIO = true;
        _eventHandler = factory.Services.GetRequiredService<IEventHandler<IssueCreated>>();
    }

    public async void Dispose()
    {
        _projectMongoDbFixture.Dispose();
        _issueMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task issue_created_event_should_add_issue_with_given_data_to_database()
    {
        var issueId = "issueKey" + Guid.NewGuid();
        var projectId = "projectKey" + Guid.NewGuid();

        var project = new Project(projectId);
        await _projectMongoDbFixture.InsertAsync(project.AsDocument());

        var externalEvent = new IssueCreated(issueId, projectId);

        // Check if exception is thrown

        await _eventHandler
            .Awaiting(c => c.HandleAsync(externalEvent))
            .Should().NotThrowAsync();


        var issue = await _issueMongoDbFixture.GetAsync(issueId);

        issue.Should().NotBeNull();
        issue.Id.Should().Be(issueId);
        issue.ProjectId.Should().Be(projectId);
        issue.SprintId.Should().BeNull();
    }

    [Fact]
    public async Task issue_created_event_fails_when_issue_with_id_already_exists()
    {
        var issueId = "issueKey" + Guid.NewGuid();
        var projectId = "projectKey" + Guid.NewGuid();

        var project = new Project(projectId);
        await _projectMongoDbFixture.InsertAsync(project.AsDocument());

        var issue = new Issue(issueId, projectId, null);
        await _issueMongoDbFixture.InsertAsync(issue.AsDocument());

        var externalEvent = new IssueCreated(issueId, projectId);

        // Check if exception is thrown

        await _eventHandler
            .Awaiting(c => c.HandleAsync(externalEvent))
            .Should().ThrowAsync<IssueAlreadyCreatedException>();
    }

    [Fact]
    public async Task issue_created_event_fails_when_project_with_id_does_not_exist()
    {
        var issueId = "issueKey" + Guid.NewGuid();
        var projectId = "projectKey" + Guid.NewGuid();

        var externalEvent = new IssueCreated(issueId, projectId);

        // Check if exception is thrown

        await _eventHandler
            .Awaiting(c => c.HandleAsync(externalEvent))
            .Should().ThrowAsync<ProjectNotFoundException>();
    }
}