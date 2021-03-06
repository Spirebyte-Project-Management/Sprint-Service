using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Events.Rejected
{
    [Contract]
    public class SprintCreatedRejected : IRejectedEvent
    {
        public string SprintId { get; }
        public string Reason { get; }
        public string Code { get; }

        public SprintCreatedRejected(string sprintId, string reason, string code)
        {
            SprintId = sprintId;
            Reason = reason;
            Code = code;
        }
    }
}
