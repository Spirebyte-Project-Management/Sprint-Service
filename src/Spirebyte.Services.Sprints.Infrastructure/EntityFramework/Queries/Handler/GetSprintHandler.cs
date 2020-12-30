using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;
using Spirebyte.Services.Sprints.Application.DTO;
using Spirebyte.Services.Sprints.Application.Queries;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables.Mappers;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Queries.Handler
{
    internal sealed class GetSprintHandler : IQueryHandler<GetSprint, SprintDto>
    {
        private readonly IEfRepository<SprintsDbContext, SprintTable, string> _sprintRepository;

        public GetSprintHandler(IEfRepository<SprintsDbContext, SprintTable, string> sprintRepository)
        {
            _sprintRepository = sprintRepository;
        }

        public async Task<SprintDto> HandleAsync(GetSprint query)
        {
            var sprint = await _sprintRepository.Collection.Include(c => c.Issues).FirstOrDefaultAsync(p => p.Id == query.SprintId);

            return sprint?.AsDto();
        }
    }
}
