using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.Application.DTO;
using Spirebyte.Services.Sprints.Application.Queries;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Queries.Handler
{
    internal sealed class GetSprintsHandler : IQueryHandler<GetSprints, IEnumerable<SprintDto>>
    {
        private readonly IEfRepository<SprintsDbContext, SprintTable, Guid> _sprintRepository;
        private readonly IEfRepository<SprintsDbContext, ProjectTable, Guid> _projectRepository;

        public GetSprintsHandler(IEfRepository<SprintsDbContext, SprintTable, Guid> sprintRepository,
            IEfRepository<SprintsDbContext, ProjectTable, Guid> projectRepository)
        {
            _sprintRepository = sprintRepository;
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<SprintDto>> HandleAsync(GetSprints query)
        {
            if (string.IsNullOrWhiteSpace(query.ProjectKey))
            {
                var sprints = await _sprintRepository.Collection.Include(c => c.Issues).ToListAsync();
                return sprints.Select(p => p.AsDto());
            }

            var project = await _projectRepository.GetAsync(p => p.Key == query.ProjectKey);

            var sprintsWithProject = await _sprintRepository.Collection.Include(c => c.Issues).Where(p => p.ProjectId == project.Id).ToListAsync();

            return sprintsWithProject.Select(p => p.AsDto());
        }
    }
}
