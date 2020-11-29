using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Sprints.Core.Exceptions;

namespace Spirebyte.Services.Sprints.Core.Entities
{
    public class Issue
    {
        public Guid Id { get; private set; }
        public string Key { get; private set; }
        public Guid ProjectId { get; private set; }
        public Guid? SprintId { get; private set; }

        public Issue(Guid id, string key, Guid projectId, Guid? sprintId)
        {
            Id = id;
            Key = key;
            ProjectId = projectId;
            SprintId = sprintId;
        }

        public void AddToSprint(Guid sprintId)
        {
            SprintId = sprintId;
        }

        public void RemoveFromSprint()
        {
            SprintId = null;
        }
    }
}
