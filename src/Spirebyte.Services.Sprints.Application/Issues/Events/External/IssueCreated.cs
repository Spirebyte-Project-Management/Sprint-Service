using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Sprints.Application.Issues.Events.External;

[Message("issues", "issue_created", "sprints.issue_created")]
public record IssueCreated(string Id, string ProjectId, int StoryPoints) : IEvent;