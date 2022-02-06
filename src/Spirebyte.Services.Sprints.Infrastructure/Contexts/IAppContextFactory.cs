using Spirebyte.Services.Sprints.Application.Contexts;

namespace Spirebyte.Services.Sprints.Infrastructure.Contexts;

public interface IAppContextFactory
{
    IAppContext Create();
}