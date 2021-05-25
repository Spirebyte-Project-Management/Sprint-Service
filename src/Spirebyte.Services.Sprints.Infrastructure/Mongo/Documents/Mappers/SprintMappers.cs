using Spirebyte.Services.Sprints.Application.DTO;
using Spirebyte.Services.Sprints.Core.Entities;
using System.Linq;

namespace Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers
{
    internal static class SprintMappers
    {
        public static Sprint AsEntity(this SprintDocument document)
            => new Sprint(document.Id, document.Title, document.Description, document.ProjectId, document.IssueIds == null ? new string[] { } : document.IssueIds.ToArray(), document.CreatedAt, document.StartedAt, document.StartDate, document.EndDate, document.EndedAt);

        public static SprintDocument AsDocument(this Sprint entity)
            => new SprintDocument
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                ProjectId = entity.ProjectId,
                CreatedAt = entity.CreatedAt,
                StartedAt = entity.StartedAt,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                EndedAt = entity.EndedAt
            };

        public static SprintDto AsDto(this SprintDocument document)
            => new SprintDto
            {
                Id = document.Id,
                Title = document.Title,
                Description = document.Description,
                ProjectId = document.ProjectId,
                IssueIds = document.IssueIds == null ? new string[] { } : document.IssueIds.ToArray(),
                CreatedAt = document.CreatedAt,
                StartedAt = document.StartedAt,
                StartDate = document.StartDate,
                EndDate = document.EndDate,
                EndedAt = document.EndedAt
            };
    }
}
