using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.Application.DTO;
using Spirebyte.Services.Sprints.Application.Exceptions;
using Spirebyte.Services.Sprints.Application.Queries;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Queries.Handler
{
    internal sealed class GetSprintsHandler : IQueryHandler<GetSprints, IEnumerable<SprintDto>>
    {
        private readonly IEfRepository<SprintsDbContext, SprintTable, string> _sprintRepository;
        private readonly IEfRepository<SprintsDbContext, ProjectTable, string> _projectRepository;

        public GetSprintsHandler(IEfRepository<SprintsDbContext, SprintTable, string> sprintRepository,
            IEfRepository<SprintsDbContext, ProjectTable, string> projectRepository)
        {
            _sprintRepository = sprintRepository;
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<SprintDto>> HandleAsync(GetSprints query)
        {
            if (string.IsNullOrWhiteSpace(query.ProjectId))
            {
                return new List<SprintDto>();
            }

            if (!await _projectRepository.ExistsAsync(p => p.Id == query.ProjectId))
            {
                throw new ProjectNotFoundException(query.ProjectId);
            }

            var project = await _projectRepository.GetAsync(query.ProjectId);

            var sprintsWithProject = await _sprintRepository.Collection.Include(c => c.Issues).Where(p => p.ProjectId == project.Id).ToListAsync();

            return sprintsWithProject.Select(p => p.AsDto());
        }
    }
}
