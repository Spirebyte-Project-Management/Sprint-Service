using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Events;

[Contract]
public class SprintUpdated : IEvent
{
    public SprintUpdated(string sprintId)
    {
        SprintId = sprintId;
    }

    public string SprintId { get; }
}