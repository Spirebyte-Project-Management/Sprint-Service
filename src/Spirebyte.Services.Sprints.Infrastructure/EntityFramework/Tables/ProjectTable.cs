using Convey.Types;
using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables
{
    public sealed class ProjectTable : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public ICollection<IssueTable> Issues { get; set; }

    }
}
