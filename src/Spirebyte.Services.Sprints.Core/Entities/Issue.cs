namespace Spirebyte.Services.Sprints.Core.Entities
{
    public class Issue
    {
        public string Id { get; private set; }
        public string ProjectId { get; private set; }
        public string? SprintId { get; private set; }

        public Issue(string id, string projectId, string? sprintId)
        {
            Id = id;
            ProjectId = projectId;
            SprintId = sprintId;
        }

        public void AddToSprint(string sprintId)
        {
            SprintId = sprintId;
        }

        public void RemoveFromSprint()
        {
            SprintId = null;
        }
    }
}
