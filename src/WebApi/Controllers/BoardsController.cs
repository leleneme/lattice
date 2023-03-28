using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lattice.WebApi.Controllers;

/// <summary>
///  Controller containing routes to manage Boards
/// </summary>
[ApiController]
[Consumes("application/json")]
[Route("api/boards", Name = "Board Endpoints")]
public class BoardController : ControllerBase
{
    private readonly ILogger<BoardController> _logger;
    private readonly IBoardService _boardService;

    public BoardController(ILogger<BoardController> logger, IBoardService boardService)
    {
        _logger = logger;
        _boardService = boardService;
    }

    /// <summary>
    ///  Creates a new Board
    /// </summary>
    /// <param name="data">Board creation payload</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful containing the Id of the newly created entity,
    ///  a 400 (BadRequest) if the payload is invalid,
    ///  a 404 (NotFound) if the TeamId references a Team that does not exists,
    ///  a 500 (InternalServerError) if a unexpected error occured in the persistence layer
    /// </returns>
    [HttpPost]
    [ModelStateFilter]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreationResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBoard([FromBody] BoardCreateDto data)
    {
        (ulong? id, BoardOperationResult result) = await _boardService.CreateAsync(data);

        return result switch
        {
            BoardOperationResult.Ok => Ok(new CreationResult(id)),
            BoardOperationResult.NotFound => NotFound(new ErrorMessage("TeamId references a User that does not exists")),
            BoardOperationResult.UnknowError => StatusCode(500),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///  Gets a specific Board using it's Id
    /// </summary>
    /// <param name="id">The Id of the Board to be fetched</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful containing Board information,
    ///  a 404 (NotFound) if the id references a Team that does not exists
    /// </returns>
    [HttpGet]
    [Route("{id:long}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BoardDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBoardById([FromRoute] ulong id)
    {
        var board = await _boardService.GetBoardInfo(id);
        return board is not null ? Ok(board) : NotFound(new ErrorMessage($"No Board with id {id} was found"));
    }

    /// <summary>
    ///  Updates a Board
    /// </summary>
    /// <param name="id">The Id of the Board to be updated</param>
    /// <param name="data">The payload contained the data to be updated in the Board</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful,
    ///  a 404 (NotFound) if the referenced Board does not exists,
    ///  a 400 (BadRequest) if the payload is invalid,
    ///  a 500 (InternalServerError) if a unexpected error occured in the persistence layer
    /// </returns>
    [HttpPut]
    [ModelStateFilter]
    [Route("{id:long}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBoard([FromRoute] ulong id, [FromBody] BoardUpdateDto data)
    {
        var result = await _boardService.UpdateAsync(id, data);
        
        return result switch
        {
            BoardOperationResult.Ok => Ok(),
            BoardOperationResult.NotFound => NotFound(new ErrorMessage($"No Board with id {id} was found")),
            BoardOperationResult.UnknowError => StatusCode(500),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///  Deletes a Board using it's Id
    /// </summary>
    /// <param name="id">The Id of the Board to be deleted</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful,
    ///  a 404 (NotFound) if the referenced Board does not exists,
    ///  a 500 (InternalServerError) if a unexpected error occured in the persistence layer
    /// </returns>
    [HttpDelete]
    [Route("{id:long}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBoard([FromRoute] ulong id)
    {
        var result = await _boardService.DeleteAsync(id);
        
        return result switch
        {
            BoardOperationResult.Ok => Ok(),
            BoardOperationResult.NotFound => NotFound(new ErrorMessage($"No Board with id {id} was found")),
            BoardOperationResult.UnknowError => StatusCode(500),
            _ => throw new UnreachableException()
        };
    }
}