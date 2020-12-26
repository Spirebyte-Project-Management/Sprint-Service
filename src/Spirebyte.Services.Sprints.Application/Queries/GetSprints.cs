using Convey.CQRS.Queries;
using Spirebyte.Services.Sprints.Application.DTO;
using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Sprints.Application.Queries
{
    public class GetSprints : IQuery<IEnumerable<SprintDto>>
    {
        public string ProjectKey { get; set; }
    }
}
