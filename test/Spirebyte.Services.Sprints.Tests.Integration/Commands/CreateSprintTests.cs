using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Framework.Tests.Shared.Infrastructure;
using Spirebyte.Services.Sprints.Application.Projects.Exceptions;
using Spirebyte.Services.Sprints.Application.Sprints.Commands;
using Spirebyte.Services.Sprints.Application.Sprints.Commands.Handlers;
using Spirebyte.Services.Sprints.Application.Sprints.Services.Interfaces;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Repositories;
using Spirebyte.Services.Sprints.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Commands;

public class CreateSprintTests : TestBase
{
    private readonly ICommandHandler<CreateSprint> _commandHandler;
    private readonly TestMessageBroker _messageBroker;

    private readonly ISprintRepository _sprintRepository;
    private readonly IProjectRepository _projectRepository;
    
    private readonly ISprintRequestStorage _sprintRequestStorage;
    
    public CreateSprintTests(
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<SprintDocument, string> sprintsMongoDbFixture) : base(projectsMongoDbFixture, issuesMongoDbFixture, sprintsMongoDbFixture)
    {
        _messageBroker = new TestMessageBroker();
        
        _sprintRepository = new SprintRepository(SprintsMongoDbFixture);
        _projectRepository = new ProjectRepository(ProjectsMongoDbFixture);

        _sprintRequestStorage = Substitute.For<ISprintRequestStorage>();
        
        _commandHandler = new CreateSprintHandler(_projectRepository, _sprintRepository, _messageBroker, _sprintRequestStorage);
    }

    [Fact]
    public async Task create_sprint_command_should_add_sprint_with_given_data_to_database()
    {
        var fakedSprint = SprintFaker.Instance.Generate();

        await ProjectsMongoDbFixture.AddAsync(new Project(fakedSprint.ProjectId).AsDocument());
        
        var command = new CreateSprint(fakedSprint.Title, fakedSprint.Description, fakedSprint.ProjectId, fakedSprint.StartDate, fakedSprint.EndDate);

        var currentSprints = await SprintsMongoDbFixture.FindAsync(r => r.ProjectId == fakedSprint.ProjectId);
        _sprintRequestStorage.SetSprint(
            Arg.Do<Guid>(r => { r.Should().Be(command.ReferenceId); }),
            Arg.Do<Sprint>(r =>
            {
                r.Should().NotBeNull();
                r.Id.Should().Be($"{fakedSprint.ProjectId}-Sprint-{currentSprints.Count + 1}");
                r.Title.Should().Be(fakedSprint.Title);
                r.Description.Should().Be(fakedSprint.Description);
                r.ProjectId.Should().Be(fakedSprint.ProjectId);
                r.StartDate.Should().Be(fakedSprint.StartDate);
                r.EndDate.Should().Be(fakedSprint.EndDate);
                r.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));
            })
        );
        
        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();
    }

    [Fact]
    public async void create_sprint_command_fails_when_project_does_not_exist()
    {
        var fakedSprint = SprintFaker.Instance.Generate();

        var command = new CreateSprint(fakedSprint.Title, fakedSprint.Description, fakedSprint.ProjectId, fakedSprint.StartDate, fakedSprint.EndDate);
        
        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<ProjectNotFoundException>();
    }
}