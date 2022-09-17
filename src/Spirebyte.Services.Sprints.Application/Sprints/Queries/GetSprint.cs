using Spirebyte.Framework.Shared.Abstractions;
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