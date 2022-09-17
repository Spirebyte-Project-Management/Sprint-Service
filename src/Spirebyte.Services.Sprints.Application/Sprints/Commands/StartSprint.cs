using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Sprints.Application.Sprints.Commands;

[Message("sprints", "start_sprint", "sprints.start_sprint")]
public class StartSprint : ICommand
{
    public StartSprint(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}