using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Events
{
    [Contract]
    public class SprintCreated : IEvent
    {
        public string SprintId { get; }

        public SprintCreated(string sprintId)
        {
            SprintId = sprintId;
        }
    }
}
