using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Lattice.WebApi.Controllers;

/// <summary>
///  Controller containing routes to manage User Accounts
/// </summary>
[ApiController]
[Consumes("application/json")]
[Route("api/users", Name = "User Endpoints")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    /// <summary>
    ///  Get a list of all Users currently on the Database
    /// </summary>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful containing the list of Users
    /// </returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDto>))]
    [ModelStateFilterAttribute]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _userService.GetAllUsers());
    }

    /// <summary>
    ///  Creates a new User Account
    /// </summary>
    /// <param name="data">User Account creation payload</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful containing the Id of the newly created entity,
    ///  a 400 (BadRequest) if the payload is invalid,
    ///  a 500 (InternalServerError) if a unexpected error occured in the persistence layer
    /// </returns>
    [HttpPost]
    [ModelStateFilter]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreationResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto data)
    {
        (ulong? uid, var result) = await _userService.CreateAsync(data);

        return result switch
        {
            UserOperationResult.Ok => Ok(new CreationResult(uid)),
            UserOperationResult.InvalidEmail => BadRequest(new ErrorMessage("Invalid E-mail address")),
            UserOperationResult.EmailAlreadyTaken => BadRequest(new ErrorMessage("E-mail address is already in use")),
            UserOperationResult.UnknowError => StatusCode(500),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///  Gets a specific User Account using it's Id
    /// </summary>
    /// <param name="uid">The Id of the User to be fetched</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful containing User information,
    ///  a 404 (NotFound) if the referenced User does not exists,
    ///  a 500 (InternalServerError) if a unexpected error occured in the persistence layer
    /// </returns>
    [HttpGet]
    [Route("{uid:long}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    public async Task<IActionResult> GetUserById([FromRoute] ulong uid)
    {
        var user = await _userService.GetUserInfo(uid);
        return user is not null ? Ok(user) : NotFound(new ErrorMessage($"No User with id {uid} was found"));
    }

    /// <summary>
    ///  Deletes a User Account using it's Id
    /// </summary>
    /// <param name="uid">The Id of the User to be deleted</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful,
    ///  a 404 (NotFound) if the referenced User does not exists,
    ///  a 500 (InternalServerError) if a unexpected error occured in the persistence layer
    /// </returns>
    [HttpDelete]
    [Route("{uid:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUserById([FromRoute] ulong uid)
    {
        var result = await _userService.DeleteAsync(uid);

        return result switch
        {
            UserOperationResult.Ok => Ok(),
            UserOperationResult.NotFound => NotFound(new ErrorMessage($"No User with id {uid} was found")),
            UserOperationResult.UnknowError => StatusCode(500),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///  Updates a User Account
    /// </summary>
    /// <param name="uid">The Id of the User to be updated</param>
    /// <param name="data">The payload contained the data to be updated</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful,
    ///  A 400 (BadRequest) if the model state is invalid,
    ///  a 404 (NotFound) if the referenced User does not exists,
    ///  a 500 (InternalServerError) if a unexpected error occured in the persistence layer
    /// </returns>
    [HttpPut]
    [Route("{uid:long}")]
    [ModelStateFilter]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser([FromRoute] ulong uid, [FromBody] UserUpdateDto data)
    {
        var result = await _userService.UpdateAsync(uid, data);

        return result switch
        {
            UserOperationResult.InvalidEmail => BadRequest("Invalid E-mail address"),
            UserOperationResult.Ok => Ok(),
            UserOperationResult.NotFound => NotFound(new ErrorMessage($"No User with id {uid} was found")),
            UserOperationResult.UnknowError => StatusCode(500), // Internal Server Error
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///  Gets all teams a User is member of
    /// </summary>
    /// <param name="uid">The Id of the User to be used to fetch it's teams</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful containing a list of teams,
    ///  a 404 (NotFound) if the referenced User does not exists,
    ///  a 500 (InternalServerError) if a unexpected error occured in the persistence layer
    /// </returns>
    [HttpGet]
    [Route("{uid:long}/teams")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TeamDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> GetUserTeams([FromRoute] ulong uid)
    {
        var teams = await _userService.GetUserTeams(uid);
        return teams is not null ? Ok(teams) : NotFound(new ErrorMessage($"No User with id {uid} was found"));
    }

    /// <summary>
    ///  Gets all cards a User is assigned to
    /// </summary>
    /// <param name="uid">The Id of the User to be used to fetch it's cards</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful containing a list of cards,
    ///  a 404 (NotFound) if the referenced User does not exists,
    ///  a 500 (InternalServerError) if a unexpected error occured in the persistence layer
    /// </returns>
    [HttpGet]
    [Route("{uid:long}/cards")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CardDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserCards([FromRoute] ulong uid)
    {
        var cards = await _userService.GetUserAssignedCards(uid);
        return cards is not null ? Ok(cards) : NotFound(new ErrorMessage($"No User with id {uid} was found"));
    }
}
