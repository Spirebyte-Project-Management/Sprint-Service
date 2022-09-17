using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Framework.Messaging;
using Spirebyte.Services.Sprints.Application.Issues.Events.External;
using Spirebyte.Services.Sprints.Application.Projects.Events.External;
using Spirebyte.Services.Sprints.Application.Sprints.Commands;
using Spirebyte.Services.Sprints.Application.Sprints.Services;
using Spirebyte.Services.Sprints.Application.Sprints.Services.Interfaces;

namespace Spirebyte.Services.Sprints.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ISprintRequestStorage, SprintRequestStorage>();

        return services;
    }

    public static IApplicationBuilder UseApplication(this IApplicationBuilder app)
    {
        app.Subscribe()
            .Command<CreateSprint>()
            .Event<ProjectCreated>()
            .Event<IssueCreated>()
            .Event<IssueUpdated>();

        return app;
    }
}