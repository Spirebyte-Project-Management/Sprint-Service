using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 1, DisableTestParallelization = true)]


namespace Spirebyte.Services.Sprints.Tests.Integration;

[CollectionDefinition(nameof(SpirebyteCollection), DisableParallelization = true)]
public class SpirebyteCollection : ICollectionFixture<DockerDbFixture>,
    ICollectionFixture<MongoDbFixture<ProjectDocument, string>>,
    ICollectionFixture<MongoDbFixture<IssueDocument, string>>,
    ICollectionFixture<MongoDbFixture<SprintDocument, string>>
{
}