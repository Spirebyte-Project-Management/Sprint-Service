using System;
using System.Threading.Tasks;
using FluentAssertions;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Framework.Tests.Shared.Infrastructure;
using Spirebyte.Services.Sprints.Application.Sprints.Commands;
using Spirebyte.Services.Sprints.Application.Sprints.Commands.Handlers;
using Spirebyte.Services.Sprints.Application.Sprints.Exceptions;
using Spirebyte.Services.Sprints.Core.Repositories;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Repositories;
using Spirebyte.Services.Sprints.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Commands;

public class EndSprintTests : TestBase
{
    private readonly ICommandHandler<EndSprint> _commandHandler;
    private readonly TestMessageBroker _messageBroker;

    private readonly ISprintRepository _sprintRepository;

    public EndSprintTests(
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<SprintDocument, string> sprintsMongoDbFixture) : base(projectsMongoDbFixture, issuesMongoDbFixture, sprintsMongoDbFixture)
    {
        _sprintRepository = new SprintRepository(SprintsMongoDbFixture);
        _messageBroker = new TestMessageBroker();

        _commandHandler = new EndSprintHandler(_sprintRepository, _messageBroker);
    }

    [Fact]
    public async Task end_sprint_command_should_set_endedAt()
    {
        var fakedSprint = SprintFaker.Instance.Generate();
            fakedSprint.StartedAt = DateTime.UtcNow;
        await SprintsMongoDbFixture.AddAsync(fakedSprint.AsDocument());

        var command = new EndSprint(fakedSprint.Id);

        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();
        
        var startedSprint = await SprintsMongoDbFixture.GetAsync(fakedSprint.Id);

        startedSprint.Should().NotBeNull();
        startedSprint.StartedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
        startedSprint.EndedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
    }

    [Fact]
    public async void end_sprint_command_fails_when_sprint_does_not_exist()
    {
        var fakedSprint = SprintFaker.Instance.Generate();
        
        var command = new EndSprint(fakedSprint.Id);

        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<SprintNotFoundException>();
    }

    [Fact]
    public async Task end_sprint_command_fails_when_sprint_has_not_been_started()
    {
        var fakedSprint = SprintFaker.Instance.Generate();
        await SprintsMongoDbFixture.AddAsync(fakedSprint.AsDocument());

        var command = new EndSprint(fakedSprint.Id);

        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<SprintNotStartedException>();
    }
}