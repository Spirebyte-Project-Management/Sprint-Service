using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.Application.DTO;
using Spirebyte.Services.Sprints.Application.Queries;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Queries.Handler
{
    internal sealed class GetSprintHandler : IQueryHandler<GetSprint, SprintDto>
    {
        private readonly IEfRepository<SprintsDbContext, SprintTable, Guid> _sprintRepository;

        public GetSprintHandler(IEfRepository<SprintsDbContext, SprintTable, Guid> sprintRepository)
        {
            _sprintRepository = sprintRepository;
        }

        public async Task<SprintDto> HandleAsync(GetSprint query)
        {
            var project = await _sprintRepository.GetAsync(p => p.Key == query.Key);

            return project?.AsDto();
        }
    }
}
