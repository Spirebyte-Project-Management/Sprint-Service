using System;
using System.Threading.Tasks;
using FluentAssertions;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Framework.Tests.Shared.Infrastructure;
using Spirebyte.Services.Sprints.Application.Issues.Exceptions;
using Spirebyte.Services.Sprints.Application.Sprints.Commands;
using Spirebyte.Services.Sprints.Application.Sprints.Commands.Handlers;
using Spirebyte.Services.Sprints.Application.Sprints.Exceptions;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Enums;
using Spirebyte.Services.Sprints.Core.Repositories;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Repositories;
using Spirebyte.Services.Sprints.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration.Commands;

public class RemoveIssueFromSprintTests : TestBase
{
    private readonly ICommandHandler<RemoveIssueFromSprint> _commandHandler;
    private readonly TestMessageBroker _messageBroker;
    
    private readonly ISprintRepository _sprintRepository;
    private readonly IIssueRepository _issueRepository;

    public RemoveIssueFromSprintTests(
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<SprintDocument, string> sprintsMongoDbFixture) : base(projectsMongoDbFixture, issuesMongoDbFixture, sprintsMongoDbFixture)
    {
        _sprintRepository = new SprintRepository(SprintsMongoDbFixture);
        _issueRepository = new IssueRepository(IssuesMongoDbFixture);
        _messageBroker = new TestMessageBroker();

        _commandHandler = new RemoveIssueFromSprintHandler(_sprintRepository, _issueRepository, _messageBroker);
    }

    [Fact]
    public async Task remove_issue_from_sprint_command_should_remove_issue_from_sprint()
    {
        var fakedSprint = SprintFaker.Instance.Generate();

        await SprintsMongoDbFixture.AddAsync(fakedSprint.AsDocument());

        var issue = new Issue("issueKey" + Guid.NewGuid(), fakedSprint.ProjectId, fakedSprint.Id, 0, IssueStatus.TODO);
        await IssuesMongoDbFixture.AddAsync(issue.AsDocument());

        var command = new RemoveIssueFromSprint(fakedSprint.Id, issue.Id);

        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();
        
        var removedIssue = await IssuesMongoDbFixture.GetAsync(issue.Id);
        removedIssue.Should().NotBeNull();
        removedIssue.SprintId.Should().BeNull();
    }

    [Fact]
    public async void remove_issue_from_sprint_command_fails_when_sprint_does_not_exist()
    {
        var fakedSprint = SprintFaker.Instance.Generate();

        var issueId = "issueKey" + Guid.NewGuid();

        var issue = new Issue(issueId, fakedSprint.ProjectId, null, 0, IssueStatus.TODO);
        await IssuesMongoDbFixture.AddAsync(issue.AsDocument());
        
        var command = new RemoveIssueFromSprint(fakedSprint.Id, issueId);
        
        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<SprintNotFoundException>();
    }

    [Fact]
    public async void remove_issue_from_sprint_command_fails_when_issue_does_not_exist()
    {
        var fakedSprint = SprintFaker.Instance.Generate();
        await SprintsMongoDbFixture.AddAsync(fakedSprint.AsDocument());

        var issueId = "issueKey" + Guid.NewGuid();
        var command = new RemoveIssueFromSprint(fakedSprint.Id, issueId);
        
        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<IssueNotFoundException>();
    }
}