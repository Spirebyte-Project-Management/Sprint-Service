using System;
using System.Threading.Tasks;
using FluentAssertions;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Services.Sprints.Application.Issues.Events.External;
using Spirebyte.Services.Sprints.Application.Issues.Events.External.Handlers;
using Spirebyte.Services.Sprints.Application.Issues.Exceptions;
using Spirebyte.Services.Sprints.Application.Projects.Exceptions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Enums;
using Spirebyte.Services.Sprints.Core.Repositories;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Repositories;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Events;

public class IssueCreatedTests : TestBase
{
    private readonly IEventHandler<IssueCreated> _eventHandler;

    private readonly IProjectRepository _projectRepository;
    private readonly IIssueRepository _issueRepository;
    
    public IssueCreatedTests(
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<SprintDocument, string> sprintsMongoDbFixture) : base(projectsMongoDbFixture, issuesMongoDbFixture, sprintsMongoDbFixture)
    {
        _projectRepository = new ProjectRepository(ProjectsMongoDbFixture);
        _issueRepository = new IssueRepository(IssuesMongoDbFixture);
        
        _eventHandler = new IssueCreatedHandler(_issueRepository, _projectRepository);
    }

    [Fact]
    public async Task issue_created_event_should_add_issue_with_given_data_to_database()
    {
        var issueId = "issueKey" + Guid.NewGuid();
        var projectId = "projectKey" + Guid.NewGuid();

        var project = new Project(projectId);
        await ProjectsMongoDbFixture.AddAsync(project.AsDocument());

        var externalEvent = new IssueCreated(issueId, projectId, 0);

        // Check if exception is thrown
        await _eventHandler
            .Awaiting(c => c.HandleAsync(externalEvent))
            .Should().NotThrowAsync();


        var issue = await IssuesMongoDbFixture.GetAsync(issueId);

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
        await ProjectsMongoDbFixture.AddAsync(project.AsDocument());

        var issue = new Issue(issueId, projectId, null, 0, IssueStatus.TODO);
        await IssuesMongoDbFixture.AddAsync(issue.AsDocument());

        var externalEvent = new IssueCreated(issueId, projectId, 0);

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

        var externalEvent = new IssueCreated(issueId, projectId, 0);

        // Check if exception is thrown
        await _eventHandler
            .Awaiting(c => c.HandleAsync(externalEvent))
            .Should().ThrowAsync<ProjectNotFoundException>();
    }
}