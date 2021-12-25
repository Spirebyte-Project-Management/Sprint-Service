namespace Spirebyte.Services.Sprints.Core.Entities;

public class Issue
{
    public Issue(string id, string projectId, string? sprintId)
    {
        Id = id;
        ProjectId = projectId;
        SprintId = sprintId;
    }

    public string Id { get; }
    public string ProjectId { get; }
    public string? SprintId { get; private set; }

    public void AddToSprint(string sprintId)
    {
        SprintId = sprintId;
    }

    public void RemoveFromSprint()
    {
        SprintId = null;
    }
}