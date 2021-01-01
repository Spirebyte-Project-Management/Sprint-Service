using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Sprints.Application.Commands
{
    [Contract]
    public class StartSprint : ICommand
    {
        public string Id { get; set; }

        public StartSprint(string id)
        {
            Id = id;
        }
    }
}
