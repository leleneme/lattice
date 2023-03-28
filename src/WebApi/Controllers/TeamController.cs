using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lattice.WebApi.Controllers;

/// <summary>
///  Controller containing routes to manage Teams
/// </summary>
[ApiController]
[Consumes("application/json")]
[Route("api/teams", Name = "Team Endpoints")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly ILogger<TeamController> _logger;

    public TeamController(ITeamService teamService, ILogger<TeamController> logger)
    {
        _teamService = teamService;
        _logger = logger;
    }

    /// <summary>
    ///  Get a list of all Teams currently on the Database
    /// </summary>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful containing the list of Teams
    /// </returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TeamDto>))]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _teamService.GetAllTeams());
    }

    /// <summary>
    ///  Creates a new Team
    /// </summary>
    /// <param name="data">Team creation payload</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful containing the Id of the newly created entity,
    ///  a 400 (BadRequest) if the payload is invalid,
    ///  a 404 (NotFound) if the team's OwnerId references a User that does not exists,
    ///  a 500 (InternalServerError) if a unexpected error occured in the persistence layer
    /// </returns>
    [HttpPost]
    [ModelStateFilter]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreationResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTeam([FromBody] TeamCreateDto data)
    {
        (ulong? id, var result) = await _teamService.CreateAsync(data);

        if (result == TeamOperationResult.Ok)
        {
            await _teamService.AddMember((ulong)id!, new UserJoinTeamDto { UserId = data.OwnerId });
        }

        return result switch
        {
            TeamOperationResult.Ok => Ok(new CreationResult(id)),
            TeamOperationResult.NotFound => Ok(new ErrorMessage("OwnerId references a User that does not exists")),
            TeamOperationResult.UnknowError => StatusCode(500),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///  Gets a specific Team using it's Id
    /// </summary>
    /// <param name="id">The Id of the Team to be fetched</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful containing Team information,
    ///  a 404 (NotFound) if the id references a Team that does not exists
    /// </returns>
    [HttpGet]
    [Route("{id:long}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeamDto))]
    public async Task<IActionResult> GetTeamById([FromRoute] ulong id)
    {
        var team = await _teamService.GetTeamInfo(id);
        return team is not null ? Ok(team) : NotFound(new ErrorMessage($"No Team with id {id} was found"));
    }

    /// <summary>
    ///  Deletes a Team using it's Id
    /// </summary>
    /// <param name="id">The Id of the Team to be deleted</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful,
    ///  a 404 (NotFound) if the referenced Team does not exists,
    ///  a 500 (InternalServerError) if a unexpected error occured in the persistence layer
    /// </returns>
    [HttpDelete]
    [Route("{id:long}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteTeamById([FromRoute] ulong id)
    {
        var result = await _teamService.DeleteAsync(id);

        return result switch
        {
            TeamOperationResult.Ok => Ok(),
            TeamOperationResult.NotFound => NotFound(new ErrorMessage($"No Team with id {id} was found")),
            TeamOperationResult.UnknowError => StatusCode(500),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///  Updates a Team
    /// </summary>
    /// <param name="id">The Id of the Team to be updated</param>
    /// <param name="data">The payload contained the data to be updated in the Team</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful,
    ///  a 404 (NotFound) if the referenced Team does not exists,
    ///  a 400 (BadRequest) if the payload is invalid,
    ///  a 500 (InternalServerError) if a unexpected error occured in the persistence layer
    /// </returns>
    [HttpPut]
    [ModelStateFilter]
    [Route("{id:long}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateTeamById([FromRoute] ulong id, [FromBody] TeamUpdateDto data)
    {
        var result = await _teamService.UpdateAsync(id, data);

        return result switch
        {
            TeamOperationResult.Ok => Ok(),
            TeamOperationResult.NotFound => NotFound(new ErrorMessage($"No Team with id {id} was found")),
            TeamOperationResult.UnknowError => StatusCode(500),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///  Get a list of a specific Team's boards
    /// </summary>
    /// <param name="id">The Id of the Team</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful containing the list of Boards of the Team,
    ///  a 404 (NotFound) if the referenced Team does not exists,
    /// </returns>
    [HttpGet]
    [Route("{id:long}/boards")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BoardDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> GetTeamBoards([FromRoute] ulong id)
    {
        var boards = await _teamService.GetTeamBoards(id);
        return boards is not null ? Ok(boards) : NotFound(new ErrorMessage($"No Team with id {id} was found"));
    }
}