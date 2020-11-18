using System;
using System.Collections.Generic;
using System.Text;
using Convey.Types;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables
{
    public class SprintTable : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime EndedAt { get; set; }
    }
}
