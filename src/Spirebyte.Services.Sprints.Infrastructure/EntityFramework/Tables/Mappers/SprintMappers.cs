using System;
using System.Linq;
using Spirebyte.Services.Sprints.Application.DTO;
using Spirebyte.Services.Sprints.Core.Entities;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers
{
    internal static class SprintMappers
    {
        public static Sprint AsEntity(this SprintTable table)
            => new Sprint(table.Id, table.Key, table.Title, table.Description, table.ProjectId, table.Issues == null ? new Guid[]{} : table.Issues.Select(c => c.Id).ToArray(), table.CreatedAt, table.StartedAt, table.StartDate, table.EndDate, table.EndedAt);

        public static SprintTable AsDocument(this Sprint entity)
            => new SprintTable
            {
                Id = entity.Id,
                Key = entity.Key,
                Title = entity.Title,
                Description = entity.Description,
                ProjectId = entity.ProjectId,
                CreatedAt = entity.CreatedAt,
                StartedAt = entity.StartedAt,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                EndedAt = entity.EndedAt
            };

        public static SprintDto AsDto(this SprintTable table)
            => new SprintDto
            {
                Id = table.Id,
                Key = table.Key,
                Title = table.Title,
                Description = table.Description,
                ProjectId = table.ProjectId,
                IssueIds = table.Issues.Select(c => c.Id).ToArray(),
                CreatedAt = table.CreatedAt,
                StartedAt = table.StartedAt,
                StartDate = table.StartDate,
                EndDate = table.EndDate,
                EndedAt = table.EndedAt
            };
    }
}
