using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Sprints.Application.Sprints.Events;

[Message("sprints", "removed_issue_from_sprint")]
internal record RemovedIssueFromSprint(string SprintId, string ProjectId, string IssueId) : IEvent;