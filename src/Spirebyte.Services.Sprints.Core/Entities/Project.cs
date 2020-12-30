using System.Collections.Generic;

namespace Spirebyte.Services.Sprints.Core.Entities
{
    public class Project
    {
        public string Id { get; private set; }
        public ICollection<Issue> Issues { get; set; }

        public Project(string id)
        {
            Id = id;
        }
    }
}
