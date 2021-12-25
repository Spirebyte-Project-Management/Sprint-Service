using System.Collections.Generic;

namespace Spirebyte.Services.Sprints.Core.Entities;

public class Project
{
    public Project(string id)
    {
        Id = id;
    }

    public string Id { get; }
    public ICollection<Issue> Issues { get; set; }
}