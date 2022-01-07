using System;
using System.Collections.Generic;
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
public class GetSprintTests : IDisposable
{
    private const string Exchange = "sprints";
    private readonly IQueryHandler<GetSprint, SprintDto> _queryHandler;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<SprintDocument, string> _sprintMongoDbFixture;

    public GetSprintTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _sprintMongoDbFixture = new MongoDbFixture<SprintDocument, string>("sprints");
        factory.Server.AllowSynchronousIO = true;
        _queryHandler = factory.Services.GetRequiredService<IQueryHandler<GetSprint, SprintDto>>();
    }

    public async void Dispose()
    {
        _sprintMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task get_sprint_query_succeeds_when_sprint_exists()
    {
        var sprintId = "sprintKey" + Guid.NewGuid();
        var title = "Title";
        var description = "description";
        var projectId = "projectKey" + Guid.NewGuid();
        var createdAt = DateTime.Now;
        var startedAt = DateTime.MinValue;
        var startDate = DateTime.MinValue;
        var endDate = DateTime.MaxValue;
        var endedAt = DateTime.MaxValue;

        var sprint = new Sprint(sprintId, title, description, projectId, null, createdAt, startedAt, startDate, endDate,
            endedAt, new List<Change>(), 0, 0);

        await _sprintMongoDbFixture.InsertAsync(sprint.AsDocument());


        var query = new GetSprint(sprintId);

        // Check if exception is thrown

        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        result.Should().NotBeNull();
        result.Id.Should().Be(sprintId);
        result.Title.Should().Be(title);
        result.Description.Should().Be(description);
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