using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Sprints.Application.Sprints.Events;

[Message("sprints", "added_issue_to_sprint")]
internal record AddedIssueToSprint(string SprintId, string ProjectId, string IssueId) : IEvent;