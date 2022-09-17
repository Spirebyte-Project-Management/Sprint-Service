using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
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

public class AddIssueToSprintTests : TestBase
{
    private readonly ICommandHandler<AddIssueToSprint> _commandHandler;
    private readonly TestMessageBroker _messageBroker;

    private readonly ISprintRepository _sprintRepository;
    private readonly IIssueRepository _issueRepository;

    private readonly ILogger<AddIssueToSprintHandler> _logger;

    public AddIssueToSprintTests(
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<SprintDocument, string> sprintsMongoDbFixture) : base(projectsMongoDbFixture, issuesMongoDbFixture, sprintsMongoDbFixture)
    {
        _sprintRepository = new SprintRepository(SprintsMongoDbFixture);
        _issueRepository = new IssueRepository(IssuesMongoDbFixture);
        _messageBroker = new TestMessageBroker();
        _logger = Substitute.For<ILogger<AddIssueToSprintHandler>>();

        _commandHandler = new AddIssueToSprintHandler(_sprintRepository, _issueRepository, _messageBroker, _logger);
    }
    
    [Fact]
    public async Task add_issue_to_sprint_command_should_ass_issue_to_sprint()
    {
        var fakedSprint = SprintFaker.Instance.Generate();
        
        await SprintsMongoDbFixture.AddAsync(fakedSprint.AsDocument());

        var issueId = "issueKey" + Guid.NewGuid();
        var issue = new Issue(issueId, fakedSprint.ProjectId, null, 0, IssueStatus.TODO);
        await IssuesMongoDbFixture.AddAsync(issue.AsDocument());
        
        var command = new AddIssueToSprint(fakedSprint.Id, issueId);

        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();
        
        var removedIssue = await IssuesMongoDbFixture.GetAsync(issueId);
        removedIssue.Should().NotBeNull();
        removedIssue.SprintId.Should().Be(fakedSprint.Id);
    }

    [Fact]
    public async void add_issue_to_sprint_command_fails_when_sprint_does_not_exist()
    {
        var fakedSprint = SprintFaker.Instance.Generate();

        var issueId = "issueKey" + Guid.NewGuid();

        var issue = new Issue(issueId, fakedSprint.ProjectId, null, 0, IssueStatus.TODO);
        await IssuesMongoDbFixture.AddAsync(issue.AsDocument());
        
        var command = new AddIssueToSprint(fakedSprint.Id, issueId);
        
        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<SprintNotFoundException>();
    }

    [Fact]
    public async void add_issue_to_sprint_command_fails_when_issue_does_not_exist()
    {
        var fakedSprint = SprintFaker.Instance.Generate();
        
        await SprintsMongoDbFixture.AddAsync(fakedSprint.AsDocument());

        var issueId = "issueKey" + Guid.NewGuid();
        
        var command = new AddIssueToSprint(fakedSprint.Id, issueId);
        
        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<IssueNotFoundException>();
    }
}