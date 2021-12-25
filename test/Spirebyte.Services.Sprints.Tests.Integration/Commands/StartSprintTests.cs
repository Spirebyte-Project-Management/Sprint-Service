using System;
using System.Threading.Tasks;
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
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Commands;

[Collection("Spirebyte collection")]
public class StartSprintTests : IDisposable
{
    private const string Exchange = "sprints";
    private readonly ICommandHandler<StartSprint> _commandHandler;
    private readonly MongoDbFixture<IssueDocument, string> _issueMongoDbFixture;
    private readonly MongoDbFixture<ProjectDocument, string> _projectMongoDbFixture;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<SprintDocument, string> _sprintMongoDbFixture;

    public StartSprintTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _sprintMongoDbFixture = new MongoDbFixture<SprintDocument, string>("sprints");
        _projectMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
        _issueMongoDbFixture = new MongoDbFixture<IssueDocument, string>("issues");
        factory.Server.AllowSynchronousIO = true;
        _commandHandler = factory.Services.GetRequiredService<ICommandHandler<StartSprint>>();
    }

    public async void Dispose()
    {
        _sprintMongoDbFixture.Dispose();
        _projectMongoDbFixture.Dispose();
        _issueMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task start_sprint_command_should_set_startedAt()
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
            endedAt);
        await _sprintMongoDbFixture.InsertAsync(sprint.AsDocument());

        var command = new StartSprint(sprintId);

        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();


        var startedSprint = await _sprintMongoDbFixture.GetAsync(sprintId);

        startedSprint.Should().NotBeNull();
        startedSprint.StartedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(10));
    }

    [Fact]
    public async void start_sprint_command_fails_when_sprint_does_not_exist()
    {
        var projectId = "projectKey" + Guid.NewGuid();

        var project = new Project(projectId);
        await _projectMongoDbFixture.InsertAsync(project.AsDocument());

        var sprintId = "sprintKey" + Guid.NewGuid();


        var command = new StartSprint(sprintId);


        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<SprintNotFoundException>();
    }
}