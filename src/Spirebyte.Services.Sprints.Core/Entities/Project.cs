using System;
using System.Collections.Generic;
using System.Linq;
using Spirebyte.Services.Sprints.Core.Entities.Base;
using Spirebyte.Services.Sprints.Core.Exceptions;

namespace Spirebyte.Services.Sprints.Core.Entities
{
    public class Project
    {
        public Guid Id { get; private set; }
        public string Key { get; private set; }
        public Project(Guid id, string key)
        {
            Id = id;
            Key = key;
        }
    }
}
