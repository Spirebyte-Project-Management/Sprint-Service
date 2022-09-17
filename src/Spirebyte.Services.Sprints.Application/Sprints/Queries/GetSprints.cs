using System.Collections.Generic;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Services.Sprints.Application.Sprints.DTO;

namespace Spirebyte.Services.Sprints.Application.Sprints.Queries;

public class GetSprints : IQuery<IEnumerable<SprintDto>>
{
    public GetSprints()
    {
    }

    public GetSprints(string? projectId = null)
    {
        ProjectId = projectId;
    }

    public string? ProjectId { get; set; }
}