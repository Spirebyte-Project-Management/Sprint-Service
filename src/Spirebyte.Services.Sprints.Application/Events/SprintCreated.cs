using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Events;

[Contract]
public class SprintCreated : IEvent
{
    public SprintCreated(string sprintId)
    {
        SprintId = sprintId;
    }

    public string SprintId { get; }
}