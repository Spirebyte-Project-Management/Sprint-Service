using Convey.CQRS.Queries;
using Spirebyte.Services.Sprints.Application.DTO;

namespace Spirebyte.Services.Sprints.Application.Queries;

public class GetSprint : IQuery<SprintDto>
{
    public GetSprint(string sprintId)
    {
        SprintId = sprintId;
    }

    public string SprintId { get; set; }
}