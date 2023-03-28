using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lattice.WebApi.Controllers;

/// <summary>
///  Controller containing routes to manage Sections
/// </summary>
[ApiController]
[Consumes("application/json")]
[Route("api/sections", Name = "Section Endpoints")]
public class SectionController : ControllerBase
{
    private readonly ILogger<SectionController> _logger;
    private readonly ISectionService _sectionService;

    public SectionController(ILogger<SectionController> logger, ISectionService sectionService)
    {
        _logger = logger;
        _sectionService = sectionService;
    }

    /// <summary>
    ///  Creates a new Section
    /// </summary>
    /// <param name="data">Board creation payload</param>
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
    public async Task<IActionResult> CreateSetion([FromBody] SectionCreateDto data)
    {
        (ulong? id, SectionOperationResult result) = await _sectionService.CreateAsync(data);

        return result switch
        {
            SectionOperationResult.Ok => Ok(new CreationResult(id)),
            SectionOperationResult.NotFound => NotFound(new ErrorMessage($"No Board with id {data.BoardId} was found")),
            SectionOperationResult.UnknowError => StatusCode(500),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///  Gets a specific Section using it's Id
    /// </summary>
    /// <param name="id">The Id of the Section to be fetched</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful containing Section information,
    ///  a 404 (NotFound) if the id references a Section that does not exists
    /// </returns>
    [HttpGet]
    [Route("{id:long}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SectionDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> GetSectionById([FromRoute] ulong id)
    {
        var section = await _sectionService.GetSectionInfo(id);
        return section is not null ? Ok(section) : NotFound(new ErrorMessage($"No Section with id {id} was found"));
    }

    /// <summary>
    ///  Updates a Section
    /// </summary>
    /// <param name="id">The Id of the Seciton to be updated</param>
    /// <param name="data">The payload contained the data to be updated in the Section</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful,
    ///  a 404 (NotFound) if the referenced Seciton does not exists,
    ///  a 500 (InternalServerError) if a unexpected error occured in the persistence layer
    /// </returns>
    [HttpPut]
    [Route("{id:long}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateSection([FromRoute] ulong id, [FromBody] SectionUpdateDto data)
    {
        var result = await _sectionService.UpdateAsync(id, data);
        
        return result switch
        {
            SectionOperationResult.Ok => Ok(),
            SectionOperationResult.NotFound => NotFound(new ErrorMessage($"No Section with id {id} was found")),
            SectionOperationResult.UnknowError => StatusCode(500),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///  Deletes a Section using it's Id
    /// </summary>
    /// <param name="id">The Id of the Section to be deleted</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful,
    ///  a 404 (NotFound) if the referenced Section does not exists,
    ///  a 500 (InternalServerError) if a unexpected error occured in the persistence layer
    /// </returns>
    [HttpDelete]
    [Route("{id:long}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteSection([FromRoute] ulong id)
    {
        var result = await _sectionService.DeleteAsync(id);
        
        return result switch
        {
            SectionOperationResult.Ok => Ok(),
            SectionOperationResult.NotFound => NotFound(new ErrorMessage($"No Section with id {id} was found")),
            SectionOperationResult.UnknowError => StatusCode(500),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///  Get a list of a specific Section's card
    /// </summary>
    /// <param name="id">The Id of the Section</param>
    /// <returns>
    ///  A responde code of 200 (OK) if the operation was successful containing the list of Cards of the Section,
    ///  a 404 (NotFound) if the referenced Section does not exists,
    /// </returns>
    [HttpGet]
    [Route("{id:long}/cards")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CardDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> GetSectionCards([FromRoute] ulong id)
    {
        var cards = await _sectionService.GetSectionCards(id);
        return cards is not null ? Ok(cards) : NotFound(new ErrorMessage($"No Section with id {id} was found"));
    }
}