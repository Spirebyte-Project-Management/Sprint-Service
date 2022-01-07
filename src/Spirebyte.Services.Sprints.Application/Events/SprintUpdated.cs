using System;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Events;

[Contract]
public class SprintUpdated : IEvent
{
    public SprintUpdated(string sprintId, DateTime startedAt, DateTime endedAt)
    {
        SprintId = sprintId;
        StartedAt = startedAt;
        EndedAt = endedAt;
    }

    public string SprintId { get; }
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }
}