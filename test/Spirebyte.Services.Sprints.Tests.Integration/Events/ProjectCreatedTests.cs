using Convey.CQRS.Events;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Sprints.API;
using Spirebyte.Services.Sprints.Application.Events.External;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;
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
            _projectMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
            factory.Server.AllowSynchronousIO = true;
            _eventHandler = factory.Services.GetRequiredService<IEventHandler<ProjectCreated>>();
        }

        public async void Dispose()
        {
            _projectMongoDbFixture.Dispose();
        }

        private const string Exchange = "sprints";
        private readonly MongoDbFixture<ProjectDocument, string> _projectMongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly IEventHandler<ProjectCreated> _eventHandler;


        [Fact]
        public async Task project_created_event_should_add_project_with_given_data_to_database()
        {
            var projectId = "projectKey" + Guid.NewGuid();

            var externalEvent = new ProjectCreated(projectId);

            // Check if exception is thrown

            _eventHandler
                .Awaiting(c => c.HandleAsync(externalEvent))
                .Should().NotThrow();


            var project = await _projectMongoDbFixture.GetAsync(projectId);

            project.Should().NotBeNull();
            project.Id.Should().Be(projectId);
        }

        [Fact]
        public async Task project_created_event_fails_when_project_with_id_already_exists()
        {
            var projectId = "projectKey" + Guid.NewGuid();

            var project = new Project(projectId);
            await _projectMongoDbFixture.InsertAsync(project.AsDocument());

            var externalEvent = new ProjectCreated(projectId);

            // Check if exception is thrown

            _eventHandler
                .Awaiting(c => c.HandleAsync(externalEvent))
                .Should().Throw<ProjectAlreadyCreatedException>();
        }
    }
}
