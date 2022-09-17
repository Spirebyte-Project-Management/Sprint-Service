using System;
using System.Threading.Tasks;
using FluentAssertions;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Services.Sprints.API;
using Spirebyte.Services.Sprints.Application.Projects.Events.External;
using Spirebyte.Services.Sprints.Application.Projects.Events.External.Handlers;
using Spirebyte.Services.Sprints.Application.Projects.Exceptions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Repositories;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Events;

public class ProjectCreatedTests : TestBase
{
    private readonly IEventHandler<ProjectCreated> _eventHandler;

    
    private readonly IProjectRepository _projectRepository;
    
    public ProjectCreatedTests(
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<SprintDocument, string> sprintsMongoDbFixture) : base(projectsMongoDbFixture, issuesMongoDbFixture, sprintsMongoDbFixture)
    {
        _projectRepository = new ProjectRepository(ProjectsMongoDbFixture);
        
        _eventHandler = new ProjectCreatedHandler(_projectRepository);
    }

    [Fact]
    public async Task project_created_event_should_add_project_with_given_data_to_database()
    {
        var projectId = "projectKey" + Guid.NewGuid();

        var externalEvent = new ProjectCreated { Id = projectId };

        // Check if exception is thrown
        await _eventHandler
            .Awaiting(c => c.HandleAsync(externalEvent))
            .Should().NotThrowAsync();

        var project = await ProjectsMongoDbFixture.GetAsync(projectId);
        project.Should().NotBeNull();
        project.Id.Should().Be(projectId);
    }

    [Fact]
    public async Task project_created_event_fails_when_project_with_id_already_exists()
    {
        var projectId = "projectKey" + Guid.NewGuid();

        var project = new Project(projectId);
        await ProjectsMongoDbFixture.AddAsync(project.AsDocument());

        var externalEvent = new ProjectCreated { Id = projectId };

        // Check if exception is thrown
        await _eventHandler
            .Awaiting(c => c.HandleAsync(externalEvent))
            .Should().ThrowAsync<ProjectAlreadyCreatedException>();
    }
}