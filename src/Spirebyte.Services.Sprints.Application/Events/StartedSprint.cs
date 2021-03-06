using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Events
{
    [Contract]
    public class StartedSprint : IEvent
    {
        public string SprintId { get; }

        public StartedSprint(string sprintId)
        {
            SprintId = sprintId;
        }
    }
}
