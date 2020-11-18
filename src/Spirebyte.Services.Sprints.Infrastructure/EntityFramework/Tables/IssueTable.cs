using System;
using Convey.Types;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables
{
    public sealed class IssueTable : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
    }
}
