using Convey.CQRS.Queries;
using Spirebyte.Services.Sprints.Application.DTO;

namespace Spirebyte.Services.Sprints.Application.Queries
{
    public class GetSprint : IQuery<SprintDto>
    {
        public string SprintId { get; set; }

        public GetSprint(string sprintId)
        {
            SprintId = sprintId;
        }

    }
}
