using Spirebyte.Services.Sprints.Core.Entities;

namespace Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;

internal static class IssueMappers
{
    public static Issue AsEntity(this IssueDocument document)
    {
        return new(document.Id, document.ProjectId, document.SprintId);
    }

    public static IssueDocument AsDocument(this Issue entity)
    {
        return new()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            SprintId = entity.SprintId
        };
    }
}