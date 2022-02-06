using Convey.CQRS.Commands;

namespace Spirebyte.Services.Sprints.Application.Sprints.Commands;

[Contract]
public class StartSprint : ICommand
{
    public StartSprint(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}