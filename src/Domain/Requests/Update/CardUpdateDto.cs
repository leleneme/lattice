namespace Lattice.Domain.Dtos;

/// <summary>
///  Data transfer object used to update Card entities (Received in requests)
/// </summary>
public record struct CardUpdateDto
{
    /// <summary>
    ///  The Section this Card belongs to (can be changed if the SectionId is valid)
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
}