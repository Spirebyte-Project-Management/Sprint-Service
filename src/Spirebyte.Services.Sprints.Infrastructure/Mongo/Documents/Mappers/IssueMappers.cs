using Spirebyte.Services.Sprints.Core.Entities;

namespace Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;

internal static class IssueMappers
{
    public static Issue AsEntity(this IssueDocument document)
    {
        return new Issue(document.Id, document.ProjectId, document.SprintId, document.StoryPoints, document.Status);
    }

    public static IssueDocument AsDocument(this Issue entity)
    {
        return new IssueDocument
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            SprintId = entity.SprintId,
            StoryPoints = entity.StoryPoints,
            Status = entity.Status
        };
    }
}