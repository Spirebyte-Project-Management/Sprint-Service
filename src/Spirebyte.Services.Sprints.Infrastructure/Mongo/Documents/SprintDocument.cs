using Convey.Types;
using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents
{
    public sealed class SprintDocument : IIdentifiable<string>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime EndedAt { get; set; }
        public IEnumerable<string> IssueIds { get; set; }
    }
}
