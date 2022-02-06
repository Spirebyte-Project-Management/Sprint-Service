using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Sprints.Application.Sprints.Commands;

[Contract]
public record CreateSprint(string Title, string Description, string ProjectId, DateTime StartDate,
    DateTime EndDate) : ICommand
{
    public DateTime CreatedAt = DateTime.Now;
    public Guid ReferenceId = Guid.NewGuid();
}