using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Events
{
    [Contract]
    public class EndedSprint : IEvent
    {
        public string SprintId { get; }

        public EndedSprint(string sprintId)
        {
            SprintId = sprintId;
        }
    }
}
