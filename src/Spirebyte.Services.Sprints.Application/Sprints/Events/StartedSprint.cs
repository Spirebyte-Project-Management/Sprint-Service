using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Sprints.Events;

[Contract]
internal record StartedSprint(string SprintId) : IEvent;