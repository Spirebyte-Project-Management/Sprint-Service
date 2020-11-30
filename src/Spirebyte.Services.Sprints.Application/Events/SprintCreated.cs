using Convey.CQRS.Events;
using System;

namespace Spirebyte.Services.Sprints.Application.Events
{
    [Contract]
    public class SprintCreated : IEvent
    {
        public Guid SprintId { get; }

        public SprintCreated(Guid sprintId)
        {
            SprintId = sprintId;
        }
    }
}
