using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.API;
using Spirebyte.Services.Sprints.Application.Commands;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Entities.Base;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers;
using Spirebyte.Services.Sprints.Tests.Shared.Factories;
using Spirebyte.Services.Sprints.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Commands
{
    [Collection("Spirebyte collection")]
    public class CreateSprintTests : IDisposable
    {
        public CreateSprintTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _sprintRepository = factory.Services.GetRequiredService<IEfRepository<SprintsDbContext, SprintTable, Guid>>();
            _projectsRepository = factory.Services.GetRequiredService<IEfRepository<SprintsDbContext, ProjectTable, Guid>>();
            _dbContext = factory.Services.GetRequiredService<SprintsDbContext>();
            factory.Server.AllowSynchronousIO = true;
            _commandHandler = factory.Services.GetRequiredService<ICommandHandler<CreateSprint>>();
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
        private readonly IEfRepository<SprintsDbContext, ProjectTable, Guid> _projectsRepository;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly SprintsDbContext _dbContext;
        private readonly ICommandHandler<CreateSprint> _commandHandler;


        [Fact]
        public async Task create_sprint_command_should_add_sprint_with_given_data_to_database()
        {
            var sprintId = new AggregateId();
            var title = "Title";
            var description = "description";
            var projectId = new AggregateId();
            var projectKey = "key";
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MaxValue;

            var project = new Project(projectId, projectKey);
            await _projectsRepository.AddAsync(project.AsDocument());


            var command = new CreateSprint(sprintId, title, description, project.Id, projectKey, startDate, endDate);

            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().NotThrow();


            var sprint = await _sprintRepository.GetAsync(command.SprintId);

            sprint.Should().NotBeNull();
            sprint.Id.Should().Be(sprintId);
            sprint.Title.Should().Be(title);
            sprint.Description.Should().Be(description);
            sprint.ProjectId.Should().Be(projectId);
            sprint.StartDate.Should().Be(startDate);
            sprint.EndDate.Should().Be(endDate);
        }

        [Fact]
        public void create_sprint_command_fails_when_project_does_not_exist()
        {
            var sprintId = new AggregateId();
            var title = "Title";
            var description = "description";
            var projectId = new AggregateId();
            var projectKey = "key";
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MaxValue;

            var command = new CreateSprint(sprintId, title, description, projectId, projectKey, startDate, endDate);


            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().Throw<ProjectNotFoundException>();
        }
    }
}
