using Spirebyte.Services.Sprints.Core.Enums;

namespace Spirebyte.Services.Sprints.Core.Entities;

public class Issue
{
    public Issue(string id, string projectId, string? sprintId, int storyPoints, IssueStatus status)
    {
        Id = id;
        ProjectId = projectId;
        SprintId = sprintId;
        StoryPoints = storyPoints;
        Status = status;
    }

    public string Id { get; }
    public string ProjectId { get; }
    public string? SprintId { get; private set; }
    public int StoryPoints { get; set; }
    public IssueStatus Status { get; set; }

    public void AddToSprint(string sprintId)
    {
        SprintId = sprintId;
    }

    public void RemoveFromSprint()
    {
        SprintId = null;
    }
}