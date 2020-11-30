using FluentAssertions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Entities.Base;
using Spirebyte.Services.Sprints.Core.Exceptions;
using System;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Unit.Core.Entities
{
    public class SprintTests
    {
        [Fact]
        public void given_valid_input_sprint_should_be_created()
        {
            var sprintId = new AggregateId();
            var key = "key-sprint-1";
            var title = "Title";
            var description = "description";
            var projectId = new AggregateId();
            var createdAt = DateTime.Now;
            var startedAt = DateTime.MinValue;
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MaxValue;
            var endedAt = DateTime.MaxValue;

            var sprint = new Sprint(sprintId, key, title, description, projectId, null, createdAt, startedAt, startDate, endDate, endedAt);

            sprint.Should().NotBeNull();
            sprint.Id.Should().Be(sprintId);
            sprint.Key.Should().Be(key);
            sprint.Title.Should().Be(title);
            sprint.Description.Should().Be(description);
            sprint.ProjectId.Should().Be(projectId);
            sprint.CreatedAt.Should().Be(createdAt);
            sprint.StartedAt.Should().Be(startedAt);
            sprint.StartDate.Should().Be(startDate);
            sprint.EndDate.Should().Be(endDate);
            sprint.EndedAt.Should().Be(endedAt);
        }


        [Fact]
        public void given_empty_key_sprint_should_throw_an_exception()
        {
            var sprintId = new AggregateId();
            var key = string.Empty;
            var title = "Title";
            var description = "description";
            var projectId = new AggregateId();
            var createdAt = DateTime.Now;
            var startedAt = DateTime.MinValue;
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MaxValue;
            var endedAt = DateTime.MaxValue;

            Action act = () => new Sprint(sprintId, key, title, description, projectId, null, createdAt, startedAt, startDate, endDate, endedAt);
            act.Should().Throw<InvalidKeyException>();
        }

        [Fact]
        public void given_empty_projectId_sprint_should_throw_an_exception()
        {
            var sprintId = new AggregateId();
            var key = "key-sprint-1";
            var title = "Title";
            var description = "description";
            var projectId = Guid.Empty;
            var createdAt = DateTime.Now;
            var startedAt = DateTime.MinValue;
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MaxValue;
            var endedAt = DateTime.MaxValue;

            Action act = () => new Sprint(sprintId, key, title, description, projectId, null, createdAt, startedAt, startDate, endDate, endedAt);
            act.Should().Throw<InvalidProjectIdException>();
        }

        [Fact]
        public void given_empty_title_project_should_throw_an_exception()
        {
            var sprintId = new AggregateId();
            var key = "key-sprint-1";
            var title = string.Empty;
            var description = "description";
            var projectId = new AggregateId();
            var createdAt = DateTime.Now;
            var startedAt = DateTime.MinValue;
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MaxValue;
            var endedAt = DateTime.MaxValue;

            Action act = () => new Sprint(sprintId, key, title, description, projectId, null, createdAt, startedAt, startDate, endDate, endedAt);
            act.Should().Throw<InvalidTitleException>();
        }
    }
}
