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
public class CreateSprintTests : IDisposable
{
    private const string Exchange = "sprints";
    private readonly ICommandHandler<CreateSprint> _commandHandler;
    private readonly MongoDbFixture<ProjectDocument, string> _projectMongoDbFixture;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<SprintDocument, string> _sprintMongoDbFixture;

    public CreateSprintTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _sprintMongoDbFixture = new MongoDbFixture<SprintDocument, string>("sprints");
        _projectMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
        factory.Server.AllowSynchronousIO = true;
        _commandHandler = factory.Services.GetRequiredService<ICommandHandler<CreateSprint>>();
    }

    public async void Dispose()
    {
        _sprintMongoDbFixture.Dispose();
        _projectMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task create_sprint_command_should_add_sprint_with_given_data_to_database()
    {
        var title = "Title";
        var description = "description";
        var projectId = "projectKey" + Guid.NewGuid();
        var startDate = DateTime.MinValue;
        var endDate = DateTime.MaxValue;

        var expectedSprintKey = $"{projectId}-Sprint-1";

        var project = new Project(projectId);
        await _projectMongoDbFixture.InsertAsync(project.AsDocument());


        var command = new CreateSprint(title, description, projectId, startDate, endDate);

        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();


        var sprint = await _sprintMongoDbFixture.GetAsync(expectedSprintKey);

        sprint.Should().NotBeNull();
        sprint.Id.Should().Be(expectedSprintKey);
        sprint.Title.Should().Be(title);
        sprint.Description.Should().Be(description);
        sprint.ProjectId.Should().Be(projectId);
        sprint.StartDate.Should().Be(startDate);
        sprint.EndDate.Should().Be(endDate);
    }

    [Fact]
    public async void create_sprint_command_fails_when_project_does_not_exist()
    {
        var title = "Title";
        var description = "description";
        var projectId = "projectId" + Guid.NewGuid();
        var startDate = DateTime.MinValue;
        var endDate = DateTime.MaxValue;

        var command = new CreateSprint(title, description, projectId, startDate, endDate);


        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<ProjectNotFoundException>();
    }
}