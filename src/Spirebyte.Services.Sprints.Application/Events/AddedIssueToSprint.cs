using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Events;

[Contract]
public record AddedIssueToSprint(string SprintId, string IssueId) : IEvent;