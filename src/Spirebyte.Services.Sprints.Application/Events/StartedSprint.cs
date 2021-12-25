﻿using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Events;

[Contract]
public class StartedSprint : IEvent
{
    public StartedSprint(string sprintId)
    {
        SprintId = sprintId;
    }

    public string SprintId { get; }
}