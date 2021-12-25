using Spirebyte.Services.Sprints.Core.Entities;

namespace Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;

internal static class ProjectMappers
{
    public static Project AsEntity(this ProjectDocument document)
    {
        return new(document.Id);
    }

    public static ProjectDocument AsDocument(this Project entity)
    {
        return new()
        {
            Id = entity.Id
        };
    }
}