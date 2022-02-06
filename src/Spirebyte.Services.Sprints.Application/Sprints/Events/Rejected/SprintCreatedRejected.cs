using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Sprints.Events.Rejected;

[Contract]
public record SprintCreatedRejected(string SprintId, string Reason, string Code) : IRejectedEvent;