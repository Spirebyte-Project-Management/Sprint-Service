using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Sprints.Core.Entities;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers
{
    internal static class IssueMappers
    {
        public static Issue AsEntity(this IssueTable table)
            => new Issue(table.Id, table.Key);

        public static IssueTable AsDocument(this Issue entity)
            => new IssueTable
            {
                Id = entity.Id,
                Key = entity.Key
            };
    }
}
