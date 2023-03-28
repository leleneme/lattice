using System.Text.Json.Serialization;

namespace Lattice.Domain.Dtos;

/// <summary>
///  Data transfer object representing a Card, to be sent by Controllers in responses
/// </summary>
public struct CardDto
{
    /// <summary>
    ///  The Id of the Card
    /// </summary>
    public ulong Id { get; set; }

    /// <summary>
    ///  The DateTime this Card was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The Section this Card belongs to
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
    ///  The UserId of whoever is assigned to this card, can be null
    /// </summary>
    /// <remarks>
    ///  By default all <b>null</b> attributes in a JSON response are ignored,
    ///  but for this attribute the condition is overridden to <b>never</b> be ignored
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public ulong? AssignedTo { get; set; }
}