using System.Threading.Tasks;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Services.Sprints.API.Controllers.Base;
using Spirebyte.Services.Sprints.Application.Sprints.Commands;
using Spirebyte.Services.Sprints.Application.Sprints.DTO;
using Spirebyte.Services.Sprints.Application.Sprints.Queries;
using Spirebyte.Services.Sprints.Application.Sprints.Services.Interfaces;
using Spirebyte.Services.Sprints.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Sprints.API.Controllers;

[Authorize]
public class SprintsController : BaseController
{
    private readonly IDispatcher _dispatcher;
    private readonly ISprintRequestStorage _sprintRequestStorage;

    public SprintsController(IDispatcher dispatcher, ISprintRequestStorage sprintRequestStorage)
    {
        _dispatcher = dispatcher;
        _sprintRequestStorage = sprintRequestStorage;
    }

    [HttpGet]
    [Authorize(ApiScopes.Read)]
    [SwaggerOperation("Browse sprints")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<SprintDto>> BrowseAsync([FromQuery] GetSprints query)
    {
        return Ok(await _dispatcher.QueryAsync(query));
    }

    [HttpGet("{sprintId}")]
    [Authorize(ApiScopes.Read)]
    [SwaggerOperation("Get Sprint")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<SprintDto>> GetAsync(string sprintId)
    {
        return OkOrNotFound(await _dispatcher.QueryAsync(new GetSprint(sprintId)));
    }

    [HttpPost]
    [Authorize(ApiScopes.Write)]
    [SwaggerOperation("Create Sprint")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateSprint(CreateSprint command)
    {
        await _dispatcher.SendAsync(command);
        var sprint = _sprintRequestStorage.GetSprint(command.ReferenceId);
        return Created($"sprints/{sprint.Id}", sprint);
    }


    [HttpPut("{sprintId}")]
    [Authorize(ApiScopes.Write)]
    [SwaggerOperation("Update sprint")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdatePermissionScheme(string sprintId, UpdateSprint command)
    {
        if (!command.Id.Equals(sprintId)) return NotFound();

        await _dispatcher.SendAsync(command);
        return Ok();
    }

    [HttpDelete("{sprintId}")]
    [Authorize(ApiScopes.Delete)]
    [SwaggerOperation("Delete sprint")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteSprint(string sprintId)
    {
        await _dispatcher.SendAsync(new DeleteSprint(sprintId));
        return Ok();
    }

    [HttpPost("{sprintId}/start")]
    [Authorize(ApiScopes.Start)]
    [SwaggerOperation("Start Sprint")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> StartSprint(string sprintId)
    {
        await _dispatcher.SendAsync(new StartSprint(sprintId));
        return Ok();
    }

    [HttpPost("{sprintId}/end")]
    [Authorize(ApiScopes.Stop)]
    [SwaggerOperation("End Sprint")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> EndSprint(string sprintId)
    {
        await _dispatcher.SendAsync(new EndSprint(sprintId));
        return Ok();
    }

    [HttpPost("{sprintId}/addIssue/{issueId}")]
    [Authorize(ApiScopes.Write)]
    [SwaggerOperation("Add issue to Sprint")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddIssueToSprint(string sprintId, string issueId)
    {
        await _dispatcher.SendAsync(new AddIssueToSprint(sprintId, issueId));
        return Ok();
    }

    [HttpPost("{sprintId}/removeIssue/{issueId}")]
    [Authorize(ApiScopes.Write)]
    [SwaggerOperation("Remove issue from sprint")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RemoveIssueFromSprint(string sprintId, string issueId)
    {
        await _dispatcher.SendAsync(new RemoveIssueFromSprint(sprintId, issueId));
        return Ok();
    }
}