using System;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Sprints.Events;

[Contract]
internal record SprintUpdated(string SprintId, DateTime StartedAt, DateTime EndedAt) : IEvent;