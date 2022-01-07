using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Events;

[Contract]
public class SprintCreated : IEvent
{
    public SprintCreated(string sprintId, string projectId)
    {
        SprintId = sprintId;
        ProjectId = projectId;
    }

    public string SprintId { get; }
    public string ProjectId { get; }
}