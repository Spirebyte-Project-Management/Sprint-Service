using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Framework.DAL.MongoDb;
using Spirebyte.Services.Sprints.Application.Clients.Interfaces;
using Spirebyte.Services.Sprints.Core.Repositories;
using Spirebyte.Services.Sprints.Infrastructure.Clients.HTTP;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Sprints.Infrastructure.Mongo.Repositories;

namespace Spirebyte.Services.Sprints.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IIdentityApiHttpClient, IdentityApiHttpClient>();

        services.AddMongo(configuration)
            .AddMongoRepository<IssueDocument, string>("issues")
            .AddMongoRepository<ProjectDocument, string>("projects")
            .AddMongoRepository<SprintDocument, string>("sprints");
        
        services.AddSingleton<ISprintRepository, SprintRepository>();
        services.AddSingleton<IProjectRepository, ProjectRepository>();
        services.AddSingleton<IIssueRepository, IssueRepository>();

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
    {
        return builder;
    }
}