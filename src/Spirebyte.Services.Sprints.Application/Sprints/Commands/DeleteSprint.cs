using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Sprints.Application.Sprints.Commands;

[Message("sprints", "delete_sprint", "sprints.delete_sprint")]
public class DeleteSprint : ICommand
{
    public DeleteSprint(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}