using Convey.CQRS.Events;
using System;

namespace Spirebyte.Services.Sprints.Application.Events.Rejected
{
    [Contract]
    public class SprintCreatedRejected : IRejectedEvent
    {
        public Guid ProjectId { get; }
        public string Reason { get; }
        public string Code { get; }

        public SprintCreatedRejected(Guid projectId, string reason, string code)
        {
            ProjectId = projectId;
            Reason = reason;
            Code = code;
        }
    }
}
