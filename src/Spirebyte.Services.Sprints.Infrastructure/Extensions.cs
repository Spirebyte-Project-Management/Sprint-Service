using System;
using System.Linq;
using Convey;
using Convey.Auth;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.Discovery.Consul;
using Convey.Docs.Swagger;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.Outbox.Mongo;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.AppMetrics;
using Convey.Persistence.MongoDB;
using Convey.Persistence.Redis;
using Convey.Security;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Convey.WebApi.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Open.Serialization.Json;
using Spirebyte.Services.Sprints.Application;
using Spirebyte.Services.Sprints.Application.Clients.Interfaces;
using Spirebyte.Services.Sprints.Application.Issues.Events.External;
using Spirebyte.Services.Sprints.Application.Projects.Events.External;
using Spirebyte.Services.Sprints.Application.Services.Interfaces;
using Spirebyte.Services.Sprints.Application.Sprints.Commands;
using Spirebyte.Services.Sprints.Core.Repositories;
using Spirebyte.Services.Sprints.Infrastructure.Clients.HTTP;
using Spirebyte.Services.Sprints.Infrastructure.Decorators;
using Spirebyte.Services.Sprints.Infrastructure.Exceptions;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Repositories;
using Spirebyte.Services.Sprints.Infrastructure.ServiceDiscovery;
using Spirebyte.Services.Sprints.Infrastructure.Services;
using Spirebyte.Shared.Contexts;

namespace Spirebyte.Services.Sprints.Infrastructure;

public static class Extensions
{
    public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
    {
        builder.Services.AddTransient<IMessageBroker, MessageBroker>();
        builder.Services.AddSingleton<ISprintRepository, SprintRepository>();
        builder.Services.AddSingleton<IProjectRepository, ProjectRepository>();
        builder.Services.AddSingleton<IIssueRepository, IssueRepository>();
        builder.Services.AddTransient<IIdentityApiHttpClient, IdentityApiHttpClient>();
        builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
        builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));

        builder.Services.AddSharedContexts();
        
        return builder
            .AddErrorHandler<ExceptionToResponseMapper>()
            .AddQueryHandlers()
            .AddInMemoryQueryDispatcher()
            .AddInMemoryDispatcher()
            .AddJwt()
            .AddHttpClient()
            .AddCustomConsul()
            .AddFabio()
            .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
            .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
            .AddMessageOutbox(o => o.AddMongo())
            .AddMongo()
            .AddRedis()
            .AddMetrics()
            .AddJaeger()
            .AddMongoRepository<IssueDocument, string>("issues")
            .AddMongoRepository<ProjectDocument, string>("projects")
            .AddMongoRepository<SprintDocument, string>("sprints")
            .AddWebApiSwaggerDocs()
            .AddSecurity();
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseErrorHandler()
            .UseSwaggerDocs()
            .UseJaeger()
            .UseConvey()
            .UseMetrics()
            .UseAccessTokenValidator()
            .UsePublicContracts<ContractAttribute>()
            .UseAuthentication()
            .UseRabbitMq()
            .SubscribeCommand<CreateSprint>()
            .SubscribeEvent<ProjectCreated>()
            .SubscribeEvent<IssueCreated>()
            .SubscribeEvent<IssueUpdated>();

        return app;
    }

    internal static CorrelationContext GetCorrelationContext(this IHttpContextAccessor accessor)
    {
        if (accessor.HttpContext is null) return null;

        if (!accessor.HttpContext.Request.Headers.TryGetValue("x-correlation-context", out var json)) return null;

        var jsonSerializer = accessor.HttpContext.RequestServices.GetRequiredService<IJsonSerializer>();
        var value = json.FirstOrDefault();

        return string.IsNullOrWhiteSpace(value) ? null : jsonSerializer.Deserialize<CorrelationContext>(value);
    }

    public static string GetUserIpAddress(this HttpContext context)
    {
        if (context is null) return string.Empty;

        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        if (context.Request.Headers.TryGetValue("x-forwarded-for", out var forwardedFor))
        {
            var ipAddresses = forwardedFor.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries);
            if (ipAddresses.Any()) ipAddress = ipAddresses[0];
        }

        return ipAddress ?? string.Empty;
    }
}