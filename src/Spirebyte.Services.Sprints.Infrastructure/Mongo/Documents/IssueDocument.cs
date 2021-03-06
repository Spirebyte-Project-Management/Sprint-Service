using Convey.Types;

namespace Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents
{
    public sealed class IssueDocument : IIdentifiable<string>
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string? SprintId { get; set; }
    }
}
