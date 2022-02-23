using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Sprints.Events;

[Contract]
internal record AddedIssueToSprint(string SprintId, string ProjectId, string IssueId) : IEvent;