using Convey;
using Convey.CQRS.Queries;
using Convey.Logging;
using Convey.Secrets.Vault;
using Convey.Types;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Sprints.Application;
using Spirebyte.Services.Sprints.Application.Commands;
using Spirebyte.Services.Sprints.Application.DTO;
using Spirebyte.Services.Sprints.Application.Queries;
using Spirebyte.Services.Sprints.Core.Repositories;
using Spirebyte.Services.Sprints.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.API
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await CreateWebHostBuilder(args)
                .Build()
                .RunAsync();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
            => WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddConvey()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    .UseInfrastructure()
                    .UsePingEndpoint()
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetSprints, IEnumerable<SprintDto>>("sprints")
                        .Get<GetIssuesWithoutSprintForProject, string[]>("issuesWithoutSprintForProject/{projectId}")
                        .Get<GetSprint, SprintDto>("sprints/{sprintId}")
                        .Post<StartSprint>("sprints/{sprintKey}/start")
                        .Post<EndSprint>("sprints/{sprintKey}/end")
                        .Post<AddIssueToSprint>("sprints/{sprintKey}/addIssue/{issueId}")
                        .Post<RemoveIssueFromSprint>("sprints/{sprintKey}/removeIssue/{issueId}")
                        .Post<CreateSprint>("sprints",
                            afterDispatch: async (cmd, ctx) =>
                            {
                                var sprint = await ctx.RequestServices.GetService<ISprintRepository>().GetLatest();
                                await ctx.Response.Created($"sprints/{sprint.Id}",
                                    await ctx.RequestServices.GetService<IQueryDispatcher>()
                                        .QueryAsync<GetSprint, SprintDto>(new GetSprint(sprint.Id)));
                            })
                    ))
                .UseLogging()
                .UseVault();
    }
}
