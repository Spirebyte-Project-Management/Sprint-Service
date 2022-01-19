using System.Linq;
using Spirebyte.Services.Sprints.Application.DTO;
using Spirebyte.Services.Sprints.Core.Entities;

namespace Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;

internal static class SprintMappers
{
    public static Sprint AsEntity(this SprintDocument document)
    {
        return new Sprint(document.Id, document.Title, document.Description, document.ProjectId,
            document.IssueIds.ToList(), document.CreatedAt,
            document.StartedAt, document.StartDate, document.EndDate, document.EndedAt, document.Changes.ToList(),
            document.RemainingStoryPoints, document.TotalStoryPoints);
    }

    public static SprintDocument AsDocument(this Sprint entity)
    {
        return new SprintDocument
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            ProjectId = entity.ProjectId,
            IssueIds = entity.IssueIds,
            CreatedAt = entity.CreatedAt,
            StartedAt = entity.StartedAt,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            EndedAt = entity.EndedAt,
            Changes = entity.Changes,
            RemainingStoryPoints = entity.RemainingStoryPoints,
            TotalStoryPoints = entity.TotalStoryPoints
        };
    }

    public static SprintDto AsDto(this SprintDocument document)
    {
        return new SprintDto
        {
            Id = document.Id,
            Title = document.Title,
            Description = document.Description,
            ProjectId = document.ProjectId,
            IssueIds = document.IssueIds.ToList(),
            CreatedAt = document.CreatedAt,
            StartedAt = document.StartedAt,
            StartDate = document.StartDate,
            EndDate = document.EndDate,
            EndedAt = document.EndedAt,
            Changes = document.Changes,
            RemainingStoryPoints = document.RemainingStoryPoints,
            TotalStoryPoints = document.TotalStoryPoints
        };
    }
}