using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Spirebyte.Services.Sprints.Application.Sprints.DTO;
using Spirebyte.Services.Sprints.Application.Sprints.Services.Interfaces;
using Spirebyte.Services.Sprints.Core.Entities;

namespace Spirebyte.Services.Sprints.Application.Sprints.Services;

public class SprintRequestStorage : ISprintRequestStorage
{
    private readonly IMemoryCache _cache;

    public SprintRequestStorage(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void SetSprint(Guid referenceId, Sprint sprint)
    {
        var issueDto = new SprintDto
        {
            Id = sprint.Id,
            Title = sprint.Title,
            Description = sprint.Description,
            ProjectId = sprint.ProjectId,
            IssueIds = sprint.IssueIds.ToList(),
            CreatedAt = sprint.CreatedAt,
            StartedAt = sprint.StartedAt,
            StartDate = sprint.StartDate,
            EndDate = sprint.EndDate,
            EndedAt = sprint.EndedAt,
            Changes = sprint.Changes,
            RemainingStoryPoints = sprint.RemainingStoryPoints,
            TotalStoryPoints = sprint.TotalStoryPoints
        };

        _cache.Set(GetKey(referenceId), issueDto, TimeSpan.FromSeconds(5));
    }

    public SprintDto GetSprint(Guid referenceId)
    {
        return _cache.Get<SprintDto>(GetKey(referenceId));
    }

    private static string GetKey(Guid commandId)
    {
        return $"sprint:{commandId:N}";
    }
}