using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;
using Spirebyte.Services.Sprints.Core.Enums;

namespace Spirebyte.Services.Sprints.Application.Issues.Events.External;

[Message("issues", "issue_updated", "sprints.issue_updated")]
public record IssueUpdated(string Id, int StoryPoints, IssueStatus Status) : IEvent;