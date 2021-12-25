using System.Collections.Generic;
using Convey.CQRS.Queries;
using Spirebyte.Services.Sprints.Application.DTO;

namespace Spirebyte.Services.Sprints.Application.Queries;

public class GetSprints : IQuery<IEnumerable<SprintDto>>
{
    public GetSprints(string? projectId = null)
    {
        ProjectId = projectId;
    }

    public string? ProjectId { get; set; }
}