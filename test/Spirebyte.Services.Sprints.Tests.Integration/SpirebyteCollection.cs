using Spirebyte.Services.Sprints.API;
using Spirebyte.Services.Sprints.Tests.Shared.Factories;
using Xunit;

namespace Spirebyte.Services.Sprints.Tests.Integration
{
    [CollectionDefinition("Spirebyte collection")]
    public class SpirebyteCollection : ICollectionFixture<SpirebyteApplicationFactory<Program>>
    {
    }
}