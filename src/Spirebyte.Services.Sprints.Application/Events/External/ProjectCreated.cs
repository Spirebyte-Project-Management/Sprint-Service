using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Sprints.Application.Events.External
{
    [Message("projects")]
    public class ProjectCreated : IEvent
    {
        public string ProjectId { get; }

        public ProjectCreated(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
