using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Services.Sprints.API;
using Spirebyte.Services.Sprints.Application.Issues.Queries;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Enums;
using Spirebyte.Services.Sprints.Core.Repositories;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Queries.Handlers;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Repositories;
using Spirebyte.Services.Sprints.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Queries;

public class GetIssuesWithoutSprintForProjectTests : TestBase
{
    private readonly IQueryHandler<GetIssuesWithoutSprintForProject, string[]> _queryHandler;
    public GetIssuesWithoutSprintForProjectTests(
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<SprintDocument, string> sprintsMongoDbFixture) : base(projectsMongoDbFixture, issuesMongoDbFixture, sprintsMongoDbFixture)
    {
        _queryHandler = new GetIssuesWithoutSprintForProjectHandler(IssuesMongoDbFixture, ProjectsMongoDbFixture);
    }
    
    [Fact]
    public async Task get_issues_without_sprint_for_project_query_succeeds_when_a_issue_without_sprint_exists()
    {
        var fakedSprint = SprintFaker.Instance.Generate();
        await ProjectsMongoDbFixture.AddAsync(new Project(fakedSprint.ProjectId).AsDocument());

        var issueId = "issueKey" + Guid.NewGuid();
        var issueWithoutSprintId = "issueWithoutSprintKey";

        var issue = new Issue(issueId, fakedSprint.ProjectId, fakedSprint.Id, 0, IssueStatus.TODO);
        var issueWithoutSprint = new Issue(issueWithoutSprintId, fakedSprint.ProjectId, null, 0, IssueStatus.TODO);
        await IssuesMongoDbFixture.AddAsync(issue.AsDocument());
        await IssuesMongoDbFixture.AddAsync(issueWithoutSprint.AsDocument());

        var query = new GetIssuesWithoutSprintForProject(fakedSprint.ProjectId);

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
        var fakedSprint = SprintFaker.Instance.Generate();
        await ProjectsMongoDbFixture.AddAsync(new Project(fakedSprint.ProjectId).AsDocument());

        var issueId = "issueKey" + Guid.NewGuid();
        var issue2Id = "issue2Key" + Guid.NewGuid();

        var issue = new Issue(issueId, fakedSprint.ProjectId, fakedSprint.Id, 0, IssueStatus.TODO);
        var issue2 = new Issue(issue2Id, fakedSprint.ProjectId, fakedSprint.Id, 0, IssueStatus.TODO);
        await IssuesMongoDbFixture.AddAsync(issue.AsDocument());
        await IssuesMongoDbFixture.AddAsync(issue2.AsDocument());

        var query = new GetIssuesWithoutSprintForProject(fakedSprint.ProjectId);
        
        // Check if exception is thrown
        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();
        result.Should().BeEmpty();
    }
}