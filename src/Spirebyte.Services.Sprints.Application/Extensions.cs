using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Sprints.Application.Sprints.Services;
using Spirebyte.Services.Sprints.Application.Sprints.Services.Interfaces;

namespace Spirebyte.Services.Sprints.Application;

public static class Extensions
{
    public static IConveyBuilder AddApplication(this IConveyBuilder builder)
    {
        builder.Services.AddSingleton<ISprintRequestStorage, SprintRequestStorage>();

        return builder
            .AddCommandHandlers()
            .AddEventHandlers()
            .AddInMemoryCommandDispatcher()
            .AddInMemoryEventDispatcher();
    }
}