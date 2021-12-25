using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Sprints.Application.Events.External;

[Message("issues")]
public class IssueCreated : IEvent
{
    public IssueCreated(string issueId, string projectId)
    {
        IssueId = issueId;
        ProjectId = projectId;
    }

    public string IssueId { get; }
    public string ProjectId { get; }
}