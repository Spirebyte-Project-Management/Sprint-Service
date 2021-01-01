using Convey.CQRS.Commands;

namespace Spirebyte.Services.Sprints.Application.Commands
{
    [Contract]
    public class EndSprint : ICommand
    {
        public string Id { get; set; }

        public EndSprint(string id)
        {
            Id = id;
        }
    }
}
