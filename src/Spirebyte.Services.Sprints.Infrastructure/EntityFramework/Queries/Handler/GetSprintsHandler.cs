using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.Application;
using Spirebyte.Services.Sprints.Application.DTO;
using Spirebyte.Services.Sprints.Application.Queries;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Queries.Handler
{
    internal sealed class GetSprintsHandler : IQueryHandler<GetSprints, IEnumerable<SprintDto>>
    {
        private readonly IEfRepository<SprintsDbContext, SprintTable, Guid> _sprintRepository;
        private readonly IAppContext _appContext;

        public GetSprintsHandler(IEfRepository<SprintsDbContext, SprintTable, Guid> sprintRepository, IAppContext appContext)
        {
            _sprintRepository = sprintRepository;
            _appContext = appContext;
        }

        public async Task<IEnumerable<SprintDto>> HandleAsync(GetSprints query)
        {
            if (query.ProjectId == Guid.Empty)
            {
                var sprints = await _sprintRepository.Collection.Include(c => c.Issues).ToListAsync();
                return sprints.Select(p => p.AsDto());
            }
            var sprintsWithProject = await _sprintRepository.Collection.Include(c => c.Issues).Where(p => p.ProjectId == query.ProjectId).ToListAsync();

            return sprintsWithProject.Select(p => p.AsDto());
        }
    }
}
