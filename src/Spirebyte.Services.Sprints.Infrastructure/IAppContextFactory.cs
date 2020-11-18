using Spirebyte.Services.Sprints.Application;

namespace Spirebyte.Services.Sprints.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}