using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Sprints.Application.Sprints.Commands;

[Message("sprints", "end_sprint", "sprints.end_sprint")]
public class EndSprint : ICommand
{
    public EndSprint(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}