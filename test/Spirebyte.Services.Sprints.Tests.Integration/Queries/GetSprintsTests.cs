using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Services.Sprints.Application.Sprints.DTO;
using Spirebyte.Services.Sprints.Application.Sprints.Queries;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Queries.Handlers;
using Spirebyte.Services.Sprints.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Queries;

public class GetSprintsTests : TestBase
{
    private readonly IQueryHandler<GetSprints, IEnumerable<SprintDto>> _queryHandler;

    public GetSprintsTests(
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<SprintDocument, string> sprintsMongoDbFixture) : base(projectsMongoDbFixture, issuesMongoDbFixture, sprintsMongoDbFixture)
    {
        _queryHandler = new GetSprintsHandler(SprintsMongoDbFixture, ProjectsMongoDbFixture);
    }

    [Fact]
    public async Task get_sprints_query_succeeds_when_a_sprint_exists()
    {
        var fakedSprints = SprintFaker.Instance.Generate(10);
        var projectId = fakedSprints.First().ProjectId;
        foreach (var fakedSprint in fakedSprints)
        {
            fakedSprint.ProjectId = projectId;
            await SprintsMongoDbFixture.AddAsync(fakedSprint.AsDocument());
        }

        await ProjectsMongoDbFixture.AddAsync(new Project(projectId).AsDocument());
        
        var query = new GetSprints(projectId);

        // Check if exception is thrown
        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        var sprintDtos = result as SprintDto[] ?? result.ToArray();
        sprintDtos.Should().BeEquivalentTo(fakedSprints,
            options => options.Excluding(c => c.ProjectId).Excluding(c => c.StartDate).Excluding(c => c.EndDate)
                .Excluding(c => c.CreatedAt));
    }

    [Fact]
    public async Task get_sprints_query_should_return_empty_when_no_sprints_exist()
    {
        var query = new GetSprints();

        // Check if exception is thrown
        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        result.Should().BeEmpty();
    }
}