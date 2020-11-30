using Convey.CQRS.Events;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.API;
using Spirebyte.Services.Sprints.Application.Events.External;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers;
using Spirebyte.Services.Sprints.Tests.Shared.Factories;
using Spirebyte.Services.Sprints.Tests.Shared.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Events
{
    [Collection("Spirebyte collection")]
    public class ProjectCreatedTests : IDisposable
    {
        public ProjectCreatedTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _projectsRepository = factory.Services.GetRequiredService<IEfRepository<SprintsDbContext, ProjectTable, Guid>>();
            _dbContext = factory.Services.GetRequiredService<SprintsDbContext>();
            factory.Server.AllowSynchronousIO = true;
            _eventHandler = factory.Services.GetRequiredService<IEventHandler<ProjectCreated>>();
        }

        public async void Dispose()
        {
            _dbContext.Sprints.RemoveRange(_dbContext.Sprints);
            _dbContext.Projects.RemoveRange(_dbContext.Projects);
            _dbContext.Issues.RemoveRange(_dbContext.Issues);
            await _dbContext.SaveChangesAsync();
        }

        private const string Exchange = "sprints";
        private readonly IEfRepository<SprintsDbContext, ProjectTable, Guid> _projectsRepository;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly SprintsDbContext _dbContext;
        private readonly IEventHandler<ProjectCreated> _eventHandler;


        [Fact]
        public async Task project_created_event_should_add_project_with_given_data_to_database()
        {
            var projectId = Guid.NewGuid();
            var key = "key";


            var externalEvent = new ProjectCreated(projectId, key);

            // Check if exception is thrown

            _eventHandler
                .Awaiting(c => c.HandleAsync(externalEvent))
                .Should().NotThrow();


            var project = await _projectsRepository.GetAsync(projectId);

            project.Should().NotBeNull();
            project.Id.Should().Be(projectId);
            project.Key.Should().Be(key);
        }

        [Fact]
        public async Task project_created_event_fails_when_project_with_id_already_exists()
        {
            var projectId = Guid.NewGuid();
            var key = "key";

            var project = new Project(projectId, key);
            await _projectsRepository.AddAsync(project.AsDocument());

            var externalEvent = new ProjectCreated(projectId, key);

            // Check if exception is thrown

            _eventHandler
                .Awaiting(c => c.HandleAsync(externalEvent))
                .Should().Throw<ProjectAlreadyCreatedException>();
        }
    }
}
