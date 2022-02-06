using Convey.CQRS.Queries;
using Spirebyte.Services.Sprints.Application.Sprints.DTO;

namespace Spirebyte.Services.Sprints.Application.Sprints.Queries;

public class GetSprint : IQuery<SprintDto>
{
    public GetSprint(string sprintId)
    {
        SprintId = sprintId;
    }

    public string SprintId { get; set; }
}