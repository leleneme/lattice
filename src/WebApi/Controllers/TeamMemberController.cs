using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lattice.WebApi.Controllers;

/// <summary>
///  Controller containing routes to manage User-Team memberships
/// </summary>
[ApiController]
[Consumes("application/json")]
[Route("api/teams", Name = "Team Members Endpoints")]
public class TeamMemberController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly ILogger<TeamMemberController> _logger;

    public TeamMemberController(ITeamService teamService, ILogger<TeamMemberController> logger)
    {
        _teamService = teamService;
        _logger = logger;
    }

    /// <summary>
    ///  Get all members of a given Team
    /// </summary>
    /// <param name="teamId">The Id of the Team</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful containing a list
    ///  of members of a team
    /// </returns>
    [HttpGet]
    [Route("{teamId}/members")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDto>))]
    public async Task<IActionResult> GetAll([FromRoute] ulong teamId)
    {
        return Ok(await _teamService.GetTeamMembers(teamId));
    }

    /// <summary>
    ///  Add a User to a specific Team
    /// </summary>
    /// <returns>
    ///  A response code of 200 (OK) if the operation is successful,
    ///  or a 400 (BadRequest) response code if the user creation payload is incorrect,
    /// </returns>
    [HttpPost]
    [Route("{teamId}/members")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDto>))]
    [ModelStateFilterAttribute]
    public async Task<IActionResult> AddMember([FromRoute] ulong teamId, [FromBody] UserJoinTeamDto data)
    {
        var result = await _teamService.AddMember(teamId, data);

        return result switch
        {
            TeamOperationResult.Ok => Ok(),
            TeamOperationResult.NotFound => NotFound(),
            TeamOperationResult.UnknowError => StatusCode(500),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///  Remove a User from a specific team
    /// </summary>
    /// <returns>
    ///  A response code of 200 (OK) if the operation is successful,
    ///  or a 400 (BadRequest) response code if the user creation payload is incorrect,
    /// </returns>
    [HttpDelete]
    [Produces("application/json")]
    [Route("{teamId}/members/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ModelStateFilterAttribute]
    public async Task<IActionResult> RemoveMember([FromRoute] ulong teamId, [FromRoute] ulong userId)
    {
        var result = await _teamService.RemoveMember(teamId, userId);

        return result switch
        {
            TeamOperationResult.Ok => Ok(),
            TeamOperationResult.NotFound => NotFound(),
            TeamOperationResult.UnknowError => StatusCode(500),
            _ => throw new UnreachableException()
        };
    }
}