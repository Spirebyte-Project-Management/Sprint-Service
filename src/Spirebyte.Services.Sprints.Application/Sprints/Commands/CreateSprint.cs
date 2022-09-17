using System;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Sprints.Application.Sprints.Commands;

[Message("sprints", "create_sprint", "sprints.create_sprint")]
public record CreateSprint(string Title, string Description, string ProjectId, DateTime StartDate,
    DateTime EndDate) : ICommand
{
    public DateTime CreatedAt = DateTime.Now;
    public Guid ReferenceId = Guid.NewGuid();
}