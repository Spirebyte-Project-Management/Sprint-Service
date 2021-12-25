using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Sprints.API;
using Spirebyte.Services.Sprints.Application.Queries;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Sprints.Tests.Shared.Factories;
using Spirebyte.Services.Sprints.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Queries;

[Collection("Spirebyte collection")]
public class GetIssuesWithoutSprintForProjectTests : IDisposable
{
    private const string Exchange = "sprints";
    private readonly MongoDbFixture<IssueDocument, string> _issueMongoDbFixture;
    private readonly MongoDbFixture<ProjectDocument, string> _projectMongoDbFixture;
    private readonly IQueryHandler<GetIssuesWithoutSprintForProject, string[]> _queryHandler;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<SprintDocument, string> _sprintMongoDbFixture;

    public GetIssuesWithoutSprintForProjectTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _sprintMongoDbFixture = new MongoDbFixture<SprintDocument, string>("sprints");
        _projectMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
        _issueMongoDbFixture = new MongoDbFixture<IssueDocument, string>("issues");
        factory.Server.AllowSynchronousIO = true;
        _queryHandler =
            factory.Services.GetRequiredService<IQueryHandler<GetIssuesWithoutSprintForProject, string[]>>();
    }

    public async void Dispose()
    {
        _sprintMongoDbFixture.Dispose();
        _projectMongoDbFixture.Dispose();
        _issueMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task get_issues_without_sprint_for_project_query_succeeds_when_a_issue_without_sprint_exists()
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

        var issueId = "issueKey" + Guid.NewGuid();
        var issueWithoutSprintId = "issueWithoutSprintKey";

        var issue = new Issue(issueId, projectId, sprintId);
        var issueWithoutSprint = new Issue(issueWithoutSprintId, projectId, null);
        await _issueMongoDbFixture.InsertAsync(issue.AsDocument());
        await _issueMongoDbFixture.InsertAsync(issueWithoutSprint.AsDocument());

        var query = new GetIssuesWithoutSprintForProject(projectId);

        // Check if exception is thrown

        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        result.Should().NotContain(issueId);
        result.Should().Contain(issueWithoutSprintId);
    }

    [Fact]
    public async Task
        get_issues_without_sprint_for_project_query_should_return_empty_when_no_issues_without_sprint_exist()
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

        var issueId = "issueKey" + Guid.NewGuid();
        var issue2Id = "issue2Key" + Guid.NewGuid();

        var issue = new Issue(issueId, projectId, sprintId);
        var issue2 = new Issue(issue2Id, projectId, sprintId);
        await _issueMongoDbFixture.InsertAsync(issue.AsDocument());
        await _issueMongoDbFixture.InsertAsync(issue2.AsDocument());

        var query = new GetIssuesWithoutSprintForProject(projectId);
        // Check if exception is thrown

        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        result.Should().BeEmpty();
    }
}