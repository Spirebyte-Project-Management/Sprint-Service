using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Spirebyte.Framework.DAL.MongoDb.Interfaces;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Sprints.Infrastructure.Mongo.Repositories;

internal sealed class SprintRepository : ISprintRepository
{
    private readonly IMongoRepository<SprintDocument, string> _repository;

    public SprintRepository(IMongoRepository<SprintDocument, string> repository)
    {
        _repository = repository;
    }

    public async Task<Sprint> GetAsync(string sprintId)
    {
        var sprint = await _repository.GetAsync(x => x.Id == sprintId);

        return sprint?.AsEntity();
    }

    public async Task<Sprint> GetLatest()
    {
        var documents = _repository.Collection.AsQueryable();

        var sprints = await documents.ToListAsync();

        var sprint = sprints.OrderByDescending(c => c.CreatedAt).FirstOrDefault();
        return sprint.AsEntity();
    }

    public Task<long> GetSprintCountOfProjectAsync(string projectId)
    {
        return _repository.Collection.CountDocumentsAsync(x => x.ProjectId == projectId);
    }

    public Task<bool> ExistsAsync(string sprintId)
    {
        return _repository.ExistsAsync(c => c.Id == sprintId);
    }

    public Task AddAsync(Sprint sprint)
    {
        return _repository.AddAsync(sprint.AsDocument());
    }

    public async Task UpdateAsync(Sprint sprint)
    {
        var sprintToUpdate = await _repository.GetAsync(sprint.Id);

        sprint.StartedAt = sprint.StartedAt != DateTime.MinValue ? sprint.StartedAt : sprintToUpdate.StartedAt;
        sprint.EndedAt = sprint.EndedAt != DateTime.MinValue ? sprint.EndedAt : sprintToUpdate.EndedAt;

        await _repository.UpdateAsync(sprint.AsDocument());
    }

    public Task DeleteAsync(string sprintId)
    {
        return _repository.DeleteAsync(sprintId);
    }
}