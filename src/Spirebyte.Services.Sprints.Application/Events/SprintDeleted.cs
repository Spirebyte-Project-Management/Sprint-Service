using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Events;

[Contract]
public class SprintDeleted : IEvent
{
    public SprintDeleted(string sprintId)
    {
        SprintId = sprintId;
    }

    public string SprintId { get; }
}