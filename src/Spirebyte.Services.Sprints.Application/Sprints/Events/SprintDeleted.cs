using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Sprints.Events;

[Contract]
internal record SprintDeleted(string SprintId) : IEvent;