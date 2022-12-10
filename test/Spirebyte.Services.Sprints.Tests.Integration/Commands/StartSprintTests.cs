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

public class StartSprintTests : TestBase
{
    private readonly ICommandHandler<StartSprint> _commandHandler;
    private readonly TestMessageBroker _messageBroker;

    private readonly ISprintRepository _sprintRepository;

    public StartSprintTests(
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<SprintDocument, string> sprintsMongoDbFixture) : base(projectsMongoDbFixture, issuesMongoDbFixture, sprintsMongoDbFixture)
    {
        _sprintRepository = new SprintRepository(SprintsMongoDbFixture);
        _messageBroker = new TestMessageBroker();

        _commandHandler = new StartSprintHandler(_sprintRepository, _messageBroker);
    }

    [Fact]
    public async Task start_sprint_command_should_set_startedAt()
    {
        var fakedSprint = SprintFaker.Instance.Generate();
        fakedSprint.StartedAt = DateTime.UtcNow;
        await SprintsMongoDbFixture.AddAsync(fakedSprint.AsDocument());

        var command = new StartSprint(fakedSprint.Id);

        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();
        
        var startedSprint = await SprintsMongoDbFixture.GetAsync(fakedSprint.Id);
        startedSprint.Should().NotBeNull();
        startedSprint.StartedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
    }

    [Fact]
    public async void start_sprint_command_fails_when_sprint_does_not_exist()
    {
        var fakedSprint = SprintFaker.Instance.Generate();

        var command = new StartSprint(fakedSprint.Id);
        
        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<SprintNotFoundException>();
    }
}