using Convey.CQRS.Queries;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.API;
using Spirebyte.Services.Sprints.Application.DTO;
using Spirebyte.Services.Sprints.Application.Queries;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers;
using Spirebyte.Services.Sprints.Tests.Shared.Factories;
using Spirebyte.Services.Sprints.Tests.Shared.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Queries
{
    [Collection("Spirebyte collection")]
    public class GetSprintsTests : IDisposable
    {
        public GetSprintsTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _sprintRepository = factory.Services.GetRequiredService<IEfRepository<SprintsDbContext, SprintTable, string>>();
            _projectRepository = factory.Services.GetRequiredService<IEfRepository<SprintsDbContext, ProjectTable, string>>();
            _dbContext = factory.Services.GetRequiredService<SprintsDbContext>();
            factory.Server.AllowSynchronousIO = true;
            _queryHandler = factory.Services.GetRequiredService<IQueryHandler<GetSprints, IEnumerable<SprintDto>>>();
        }

        public async void Dispose()
        {
            _dbContext.Sprints.RemoveRange(_dbContext.Sprints);
            _dbContext.Projects.RemoveRange(_dbContext.Projects);
            _dbContext.Issues.RemoveRange(_dbContext.Issues);
            await _dbContext.SaveChangesAsync();
        }

        private const string Exchange = "sprints";
        private readonly IEfRepository<SprintsDbContext, SprintTable, string> _sprintRepository;
        private readonly IEfRepository<SprintsDbContext, ProjectTable, string> _projectRepository;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly SprintsDbContext _dbContext;
        private readonly IQueryHandler<GetSprints, IEnumerable<SprintDto>> _queryHandler;


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
            await _projectRepository.AddAsync(project.AsDocument());

            var sprint = new Sprint(sprintId, title, description, projectId, null, createdAt, startedAt, startDate, endDate, endedAt);
            var sprint2 = new Sprint(sprint2Id, title, description, projectId, null, createdAt, startedAt, startDate, endDate, endedAt);

            await _sprintRepository.AddAsync(sprint.AsDocument());
            await _sprintRepository.AddAsync(sprint2.AsDocument());


            var query = new GetSprints(projectId);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

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

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().BeEmpty();
        }
    }
}
