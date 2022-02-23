using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Sprints.Application.Issues.Events.External;

[Message("issues")]
public record IssueCreated(string Id, string ProjectId, int StoryPoints) : IEvent;