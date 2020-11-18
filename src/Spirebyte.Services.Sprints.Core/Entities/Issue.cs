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
        public Issue(Guid id, string key)
        {
            Id = id;
            Key = key;
        }
    }
}
