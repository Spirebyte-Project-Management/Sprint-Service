using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Framework;
using Spirebyte.Framework.Auth;
using Spirebyte.Services.Sprints.Application;
using Spirebyte.Services.Sprints.Core.Constants;
using Spirebyte.Services.Sprints.Infrastructure;

namespace Spirebyte.Services.Sprints.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateWebHostBuilder(args)
            .Build()
            .RunAsync();
    }

    private static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args)
            .ConfigureServices((ctx, services) => services
                .AddApplication()
                .AddInfrastructure(ctx.Configuration)
                .Configure<AuthorizationOptions>(options =>
                {
                    options.AddEitherOrScopePolicy(ApiScopes.SprintsRead, ApiScopes.SprintsRead, ApiScopes.SprintsManage);
                    options.AddEitherOrScopePolicy(ApiScopes.SprintsWrite, ApiScopes.SprintsWrite, ApiScopes.SprintsManage);
                    options.AddEitherOrScopePolicy(ApiScopes.SprintsDelete, ApiScopes.SprintsDelete, ApiScopes.SprintsManage);
                    options.AddEitherOrScopePolicy(ApiScopes.SprintsStart, ApiScopes.SprintsStart, ApiScopes.SprintsManage);
                    options.AddEitherOrScopePolicy(ApiScopes.SprintsStop, ApiScopes.SprintsStop, ApiScopes.SprintsManage);
                })
                .AddControllers()
            )
            .Configure(app => app
                .UseSpirebyteFramework()
                .UseApplication()
                .UseInfrastructure()
                .UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("",
                            ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppInfo>().Name));
                        endpoints.MapGet("/ping", () => "pong");
                        endpoints.MapControllers();
                    }
                ))
            .AddSpirebyteFramework();
    }
}