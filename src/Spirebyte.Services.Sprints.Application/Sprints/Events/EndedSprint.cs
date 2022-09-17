using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Sprints.Application.Sprints.Events;

[Message("sprints", "ended_sprint")]
internal record EndedSprint(string SprintId, string ProjectId) : IEvent;