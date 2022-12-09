using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Framework.API;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Sprints.Application.Sprints.Commands;
using Spirebyte.Services.Sprints.Application.Sprints.DTO;
using Spirebyte.Services.Sprints.Application.Sprints.Queries;
using Spirebyte.Services.Sprints.Application.Sprints.Services.Interfaces;
using Spirebyte.Services.Sprints.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Sprints.API.Controllers;

[Authorize]
public class SprintsController : ApiController
{
    private readonly IDispatcher _dispatcher;
    private readonly ISprintRequestStorage _sprintRequestStorage;

    public SprintsController(IDispatcher dispatcher, ISprintRequestStorage sprintRequestStorage)
    {
        _dispatcher = dispatcher;
        _sprintRequestStorage = sprintRequestStorage;
    }

    [HttpGet]
    [Authorize(ApiScopes.SprintsRead)]
    [SwaggerOperation("Browse sprints")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<SprintDto>> BrowseAsync([FromQuery] GetSprints query)
    {
        return Ok(await _dispatcher.QueryAsync(query));
    }

    [HttpGet("{sprintId}")]
    [Authorize(ApiScopes.SprintsRead)]
    [SwaggerOperation("Get Sprint")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SprintDto?>> GetAsync(string sprintId)
    {
        return await _dispatcher.QueryAsync(new GetSprint(sprintId));
    }

    [HttpPost]
    [Authorize(ApiScopes.SprintsWrite)]
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
    [Authorize(ApiScopes.SprintsWrite)]
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
    [Authorize(ApiScopes.SprintsDelete)]
    [SwaggerOperation("Delete sprint")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteSprint(string sprintId)
    {
        await _dispatcher.SendAsync(new DeleteSprint(sprintId));
        return Ok();
    }

    [HttpPost("{sprintId}/start")]
    [Authorize(ApiScopes.SprintsStart)]
    [SwaggerOperation("Start Sprint")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> StartSprint(string sprintId)
    {
        await _dispatcher.SendAsync(new StartSprint(sprintId));
        return Ok();
    }

    [HttpPost("{sprintId}/end")]
    [Authorize(ApiScopes.SprintsStop)]
    [SwaggerOperation("End Sprint")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> EndSprint(string sprintId)
    {
        await _dispatcher.SendAsync(new EndSprint(sprintId));
        return Ok();
    }

    [HttpPost("{sprintId}/addIssue/{issueId}")]
    [Authorize(ApiScopes.SprintsWrite)]
    [SwaggerOperation("Add issue to Sprint")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddIssueToSprint(string sprintId, string issueId)
    {
        await _dispatcher.SendAsync(new AddIssueToSprint(sprintId, issueId));
        return Ok();
    }

    [HttpPost("{sprintId}/removeIssue/{issueId}")]
    [Authorize(ApiScopes.SprintsWrite)]
    [SwaggerOperation("Remove issue from sprint")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RemoveIssueFromSprint(string sprintId, string issueId)
    {
        await _dispatcher.SendAsync(new RemoveIssueFromSprint(sprintId, issueId));
        return Ok();
    }
}