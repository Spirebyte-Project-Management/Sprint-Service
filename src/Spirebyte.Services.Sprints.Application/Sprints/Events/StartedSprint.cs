using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Sprints.Application.Sprints.Events;

[Message("sprints", "started_sprint")]
internal record StartedSprint(string SprintId, string ProjectId) : IEvent;