using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lattice.WebApi.Controllers;

/// <summary>
///  Controller containing routes to manage Cards
/// </summary>
[ApiController]
[Consumes("application/json")]
[Route("api/cards", Name = "Card Endpoints")]
public class CardController : ControllerBase
{
    private readonly ILogger<CardController> _logger;
    private readonly ICardService _cardService;

    public CardController(ILogger<CardController> logger, ICardService cardService)
    {
        _logger = logger;
        _cardService = cardService;
    }

    /// <summary>
    ///  Creates a new Card
    /// </summary>
    /// <param name="data">Card creation payload</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful containing the Id of the newly created entity,
    ///  a 400 (BadRequest) if the payload is invalid,
    ///  a 404 (NotFound) if the card's Section references a Section that does not exists,
    ///  a 500 (InternalServerError) if a unexpected error occured in the persistence layer
    /// </returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreationResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCard([FromBody] CardCreateDto data)
    {
        (ulong? id, CardOperationResult result) = await _cardService.CreateAsync(data);
        
        return result switch
        {
            CardOperationResult.Ok => Ok(new CreationResult(id)),
            CardOperationResult.NotFound => NotFound(new ErrorMessage("SectionId references a Section that does not exists")),
            CardOperationResult.UnknowError => StatusCode(500),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///  Assigns a Card to a User
    /// </summary>
    /// <param name="cardId">The Id of the Card</param>
    /// <param name="userId">The Id of the User</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful,
    ///  a 400 (BadRequest) if the payload is invalid,
    ///  a 404 (NotFound) if the userId references a user that does not exists or the cardId references a non-existing card,
    ///  a 500 (InternalServerError) if a unexpected error occured in the persistence layer
    /// </returns>
    [HttpPost]
    [Produces("application/json")]
    [Route("{cardId:long}/assignTo/{userId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AssignCardToUser([FromRoute] ulong cardId, [FromRoute] ulong userId)
    {
        CardOperationResult result = await _cardService.AssignCardTo(cardId, userId);
        
        return result switch
        {
            CardOperationResult.Ok => Ok(),
            CardOperationResult.NotFound => NotFound(new ErrorMessage($"No Card with id {cardId} was found")),
            CardOperationResult.UserNotFound => NotFound(new ErrorMessage($"No User with id {userId} was found")),
            CardOperationResult.UnknowError => StatusCode(500),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///  Gets a specific Card using it's Id
    /// </summary>
    /// <param name="id">The Id of the Card to be fetched</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful containing Card information,
    ///  a 404 (NotFound) if the id references a Card that does not exists
    /// </returns>
    [HttpGet]
    [Route("{id:long}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CardDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> GetCardById([FromRoute] ulong id)
    {
        var card = await _cardService.GetCardInfo(id);
        return card is not null ? Ok(card) : NotFound(new ErrorMessage($"No Card with id {id} was found"));
    }

    /// <summary>
    ///  Updates a Card
    /// </summary>
    /// <param name="id">The Id of the Card to be updated</param>
    /// <param name="data">The payload contained the data to be updated in the Card</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful,
    ///  a 404 (NotFound) if the referenced Card does not exists,
    ///  a 500 (InternalServerError) if a unexpected error occured in the persistence layer
    /// </returns>
    [HttpPut]
    [Route("{id:long}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCard([FromRoute] ulong id, [FromBody] CardUpdateDto data)
    {
        var result = await _cardService.UpdateAsync(id, data);
        
        return result switch
        {
            CardOperationResult.Ok => Ok(),
            CardOperationResult.NotFound => NotFound(new ErrorMessage($"No Card with id {id} was found")),
            CardOperationResult.UnknowError => StatusCode(500),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///  Deletes a Card using it's Id
    /// </summary>
    /// <param name="id">The Id of the Card to be deleted</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful,
    ///  a 404 (NotFound) if the referenced Card does not exists,
    ///  a 500 (InternalServerError) if a unexpected error occured in the persistence layer
    /// </returns>
    [HttpDelete]
    [Route("{id:long}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCard([FromRoute] ulong id)
    {
        var result = await _cardService.DeleteAsync(id);
        
        return result switch
        {
            CardOperationResult.Ok => Ok(),
            CardOperationResult.NotFound => NotFound(new ErrorMessage($"No Card with id {id} was found")),
            CardOperationResult.UnknowError => StatusCode(500),
            _ => throw new UnreachableException()
        };
    }
}