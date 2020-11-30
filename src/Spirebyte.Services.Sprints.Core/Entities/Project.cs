using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Sprints.Core.Entities
{
    public class Project
    {
        public Guid Id { get; private set; }
        public string Key { get; private set; }
        public ICollection<Issue> Issues { get; set; }

        public Project(Guid id, string key)
        {
            Id = id;
            Key = key;
        }
    }
}
