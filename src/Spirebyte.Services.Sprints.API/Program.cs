using System.Threading.Tasks;
using Convey;
using Convey.Logging;
using Convey.Secrets.Vault;
using Convey.Types;
using Convey.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Sprints.Application;
using Spirebyte.Services.Sprints.Core.Constants;
using Spirebyte.Services.Sprints.Infrastructure;
using Spirebyte.Shared.IdentityServer;

namespace Spirebyte.Services.Sprints.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateWebHostBuilder(args)
            .Build()
            .RunAsync();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddControllers().AddMetrics();
                services.AddAuthorization(options =>
                {
                    options.AddEitherOrScopePolicy(ApiScopes.Read, "sprints.read", "sprints.manage");
                    options.AddEitherOrScopePolicy(ApiScopes.Write, "sprints.write", "sprints.manage");
                    options.AddEitherOrScopePolicy(ApiScopes.Delete, "sprints.delete", "sprints.manage");
                    options.AddEitherOrScopePolicy(ApiScopes.Start, "sprints.start", "sprints.manage");
                    options.AddEitherOrScopePolicy(ApiScopes.Stop, "sprints.stop", "sprints.manage");
                });
                services
                    .AddConvey()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build();
            })
            .Configure(app => app
                .UseInfrastructure()
                .UseRouting()
                .UseAuthorization()
                .UsePingEndpoint()
                .UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("",
                            ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>()?.Name!));
                        endpoints.MapControllers();
                    }
                ))
            .UseLogging()
            .UseVault();
    }
}