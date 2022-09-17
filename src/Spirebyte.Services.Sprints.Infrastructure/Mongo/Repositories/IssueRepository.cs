using System.Threading.Tasks;
using Spirebyte.Framework.DAL.MongoDb.Interfaces;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Core.Repositories;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Sprints.Infrastructure.Mongo.Repositories;

internal sealed class IssueRepository : IIssueRepository
{
    private readonly IMongoRepository<IssueDocument, string> _repository;

    public IssueRepository(IMongoRepository<IssueDocument, string> repository)
    {
        _repository = repository;
    }

    public async Task<Issue> GetAsync(string id)
    {
        var issue = await _repository.GetAsync(id);

        return issue?.AsEntity();
    }

    public Task<bool> ExistsAsync(string id)
    {
        return _repository.ExistsAsync(c => c.Id == id);
    }

    public Task AddAsync(Issue issue)
    {
        return _repository.AddAsync(issue.AsDocument());
    }

    public Task UpdateAsync(Issue issue)
    {
        return _repository.UpdateAsync(issue.AsDocument());
    }
}