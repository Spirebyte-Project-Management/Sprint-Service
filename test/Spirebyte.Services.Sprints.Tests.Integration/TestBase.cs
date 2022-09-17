using System;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration;

[Collection(nameof(SpirebyteCollection))]
public class TestBase : IDisposable
{
    protected readonly MongoDbFixture<ProjectDocument, string> ProjectsMongoDbFixture;
    protected readonly MongoDbFixture<IssueDocument, string> IssuesMongoDbFixture;
    protected readonly MongoDbFixture<SprintDocument, string> SprintsMongoDbFixture;

    public TestBase(
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<SprintDocument, string> sprintsMongoDbFixture)
    {
        ProjectsMongoDbFixture = projectsMongoDbFixture;
        IssuesMongoDbFixture = issuesMongoDbFixture;
        SprintsMongoDbFixture = sprintsMongoDbFixture;
    }
    
    public void Dispose()
    {
        ProjectsMongoDbFixture.Dispose();
        IssuesMongoDbFixture.Dispose();
        SprintsMongoDbFixture.Dispose();
    }
}