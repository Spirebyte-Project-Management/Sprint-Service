using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Services.Sprints.API;
using Spirebyte.Services.Sprints.Application.Sprints.DTO;
using Spirebyte.Services.Sprints.Application.Sprints.Queries;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Queries.Handlers;
using Spirebyte.Services.Sprints.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Queries;

public class GetSprintTests : TestBase
{
    private readonly IQueryHandler<GetSprint, SprintDto> _queryHandler;

    public GetSprintTests(
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<SprintDocument, string> sprintsMongoDbFixture) : base(projectsMongoDbFixture, issuesMongoDbFixture, sprintsMongoDbFixture)
    {
        _queryHandler = new GetSprintHandler(SprintsMongoDbFixture);
    }

    [Fact]
    public async Task get_sprint_query_succeeds_when_sprint_exists()
    {
        var fakedSprint = SprintFaker.Instance.Generate();
        
        await SprintsMongoDbFixture.AddAsync(fakedSprint.AsDocument());
        
        var query = new GetSprint(fakedSprint.Id);

        // Check if exception is thrown
        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        result.Should().NotBeNull();
        result.Id.Should().Be(fakedSprint.Id);
        result.Title.Should().Be(fakedSprint.Title);
        result.Description.Should().Be(fakedSprint.Description);
    }

    [Fact]
    public async Task get_sprint_query_should_return_null_when_sprint_does_not_exist()
    {
        var sprintId = "notExistingSprintKey";

        var query = new GetSprint(sprintId);

        // Check if exception is thrown
        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();
        result.Should().BeNull();
    }
}