namespace Lattice.Domain.Dtos;

/// <summary>
///  Data transfer object used to create Card entities (Received in requests)
/// </summary>
public record struct CardCreateDto
{
    /// <summary>
    ///  The Section this Card belongs to
    /// </summary>
    public ulong SectionId { get; set; }

    /// <summary>
    ///  Title of the Card
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///  Description body of a Card
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    ///  Status of a Card:
    ///     0 - Todo,
    ///     1 - Commited,
    ///     2 - OnHold,
    ///     3 - Completed,
    ///     4 - Dropped
    /// </summary>
    public CardStatus Status { get; set; }

    /// <summary>
    ///  The UserId of whoever created this Card, must be valid on creation
    /// </summary>
    public ulong CreatedBy { get; set; }

    /// <summary>
    ///  The UserId of whoever is assigned to this card, can be null
    /// </summary>
    public ulong? AssignedTo { get; set; }
}