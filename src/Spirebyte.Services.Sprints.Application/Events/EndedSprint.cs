using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Events;

[Contract]
public class EndedSprint : IEvent
{
    public EndedSprint(string sprintId)
    {
        SprintId = sprintId;
    }

    public string SprintId { get; }
}