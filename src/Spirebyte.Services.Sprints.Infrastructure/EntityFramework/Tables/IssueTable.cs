using Convey.Types;
using System;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables
{
    public sealed class IssueTable : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectTable Project { get; set; }
        public Guid? SprintId { get; set; }
        public SprintTable Sprint { get; set; }
    }
}
