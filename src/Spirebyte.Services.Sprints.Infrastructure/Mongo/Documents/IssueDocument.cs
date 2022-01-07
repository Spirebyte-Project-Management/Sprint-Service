using Convey.Types;
using Spirebyte.Services.Sprints.Core.Enums;

namespace Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;

public sealed class IssueDocument : IIdentifiable<string>
{
    public string ProjectId { get; set; }
    public string? SprintId { get; set; }
    public int StoryPoints { get; set; }
    public IssueStatus Status { get; set; }
    public string Id { get; set; }
}