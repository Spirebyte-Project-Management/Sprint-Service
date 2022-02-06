using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Spirebyte.Services.Sprints.Core.Enums;

namespace Spirebyte.Services.Sprints.Application.Issues.Events.External;

[Message("issues")]
public record IssueUpdated(string IssueId, int StoryPoints, IssueStatus Status) : IEvent;