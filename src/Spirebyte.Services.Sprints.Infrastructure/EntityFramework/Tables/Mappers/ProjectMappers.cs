using Spirebyte.Services.Sprints.Core.Entities;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers
{
    internal static class ProjectMappers
    {
        public static Project AsEntity(this ProjectTable table)
            => new Project(table.Id, table.Key);

        public static ProjectTable AsDocument(this Project entity)
            => new ProjectTable
            {
                Id = entity.Id,
                Key = entity.Key
            };
    }
}
