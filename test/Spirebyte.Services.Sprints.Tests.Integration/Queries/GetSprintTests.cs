using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.API;
using Spirebyte.Services.Sprints.Application.DTO;
using Spirebyte.Services.Sprints.Application.Queries;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Entities.Base;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers;
using Spirebyte.Services.Sprints.Tests.Shared.Factories;
using Spirebyte.Services.Sprints.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Queries
{
    [Collection("Spirebyte collection")]
    public class GetSprintTests : IDisposable
    {
        public GetSprintTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _sprintRepository = factory.Services.GetRequiredService<IEfRepository<SprintsDbContext, SprintTable, Guid>>();
            _dbContext = factory.Services.GetRequiredService<SprintsDbContext>();
            factory.Server.AllowSynchronousIO = true;
            _queryHandler = factory.Services.GetRequiredService<IQueryHandler<GetSprint, SprintDto>>();
        }

        public async void Dispose()
        {
            _dbContext.Sprints.RemoveRange(_dbContext.Sprints);
            _dbContext.Projects.RemoveRange(_dbContext.Projects);
            _dbContext.Issues.RemoveRange(_dbContext.Issues);
            await _dbContext.SaveChangesAsync();
        }

        private const string Exchange = "sprints";
        private readonly IEfRepository<SprintsDbContext, SprintTable, Guid> _sprintRepository;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly SprintsDbContext _dbContext;
        private readonly IQueryHandler<GetSprint, SprintDto> _queryHandler;


        [Fact]
        public async Task get_sprint_query_succeeds_when_sprint_exists()
        {
            var sprintId = Guid.NewGuid();
            var key = "key-sprint-1";
            var title = "Title";
            var description = "description";
            var projectId = Guid.NewGuid();
            var createdAt = DateTime.Now;
            var startedAt = DateTime.MinValue;
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MaxValue;
            var endedAt = DateTime.MaxValue;

            var sprint = new Sprint(sprintId, key, title, description, projectId, createdAt, startedAt, startDate, endDate, endedAt);

            await _sprintRepository.AddAsync(sprint.AsDocument());


            var query = new GetSprint(key);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().NotBeNull();
            result.Id.Should().Be(sprintId);
            result.Title.Should().Be(title);
            result.Description.Should().Be(description);
        }

        [Fact]
        public async Task get_sprint_query_should_return_null_when_sprint_does_not_exist()
        {
            var key = "notExistingKey";

            var query = new GetSprint(key);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().BeNull();
        }
    }
}
