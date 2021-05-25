using Convey.Types;

namespace Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents
{
    public sealed class ProjectDocument : IIdentifiable<string>
    {
        public string Id { get; set; }
    }
}
