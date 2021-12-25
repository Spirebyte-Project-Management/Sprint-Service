using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Sprints.API;
using Spirebyte.Services.Sprints.Application.DTO;
using Spirebyte.Services.Sprints.Application.Queries;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Sprints.Tests.Shared.Factories;
using Spirebyte.Services.Sprints.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Queries;

[Collection("Spirebyte collection")]
public class GetSprintsTests : IDisposable
{
    private const string Exchange = "sprints";
    private readonly MongoDbFixture<ProjectDocument, string> _projectMongoDbFixture;
    private readonly IQueryHandler<GetSprints, IEnumerable<SprintDto>> _queryHandler;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<SprintDocument, string> _sprintMongoDbFixture;

    public GetSprintsTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _sprintMongoDbFixture = new MongoDbFixture<SprintDocument, string>("sprints");
        _projectMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
        factory.Server.AllowSynchronousIO = true;
        _queryHandler = factory.Services.GetRequiredService<IQueryHandler<GetSprints, IEnumerable<SprintDto>>>();
    }

    public async void Dispose()
    {
        _sprintMongoDbFixture.Dispose();
        _projectMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task get_sprints_query_succeeds_when_a_sprint_exists()
    {
        var sprintId = "sprintKey" + Guid.NewGuid();
        var sprint2Id = "sprint2Key" + Guid.NewGuid();
        var title = "Title";
        var description = "description";
        var projectId = "projectKey" + Guid.NewGuid();
        var createdAt = DateTime.Now;
        var startedAt = DateTime.MinValue;
        var startDate = DateTime.MinValue;
        var endDate = DateTime.MaxValue;
        var endedAt = DateTime.MaxValue;

        var project = new Project(projectId);
        await _projectMongoDbFixture.InsertAsync(project.AsDocument());

        var sprint = new Sprint(sprintId, title, description, projectId, null, createdAt, startedAt, startDate, endDate,
            endedAt);
        var sprint2 = new Sprint(sprint2Id, title, description, projectId, null, createdAt, startedAt, startDate,
            endDate, endedAt);

        await _sprintMongoDbFixture.InsertAsync(sprint.AsDocument());
        await _sprintMongoDbFixture.InsertAsync(sprint2.AsDocument());


        var query = new GetSprints(projectId);

        // Check if exception is thrown

        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        var sprintDtos = result as SprintDto[] ?? result.ToArray();
        sprintDtos.Should().Contain(i => i.Id == sprintId);
        sprintDtos.Should().Contain(i => i.Id == sprint2Id);
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