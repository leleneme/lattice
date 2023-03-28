namespace Lattice.Domain.Entities;

/// <summary>
///  A Card defines a task that can be assigned to a User and belongs to a Section, which in turn belongs to a Bord.
///  It has a status enum with possible states.
/// </summary>
[Table("card")]
public class Card : BaseEntity
{
    /// <summary>
    ///  Title of the card.
    /// </summary>
    [Column("name")]
    public required string Name { get; set; } 

    /// <summary>
    ///  Describes the card with a maximum of 300 characters.
    /// </summary>
    [Column("description")]
    [MaxLength(300)]
    public required string Description { get; set; } 

    /// <summary>
    ///  References the ID of the User this card is assigned.
    ///  This may be null, i.e. the card isn't assigned to anyone yet.
    /// </summary>
    [Column("assigned_to")]
    public ulong? AssignedTo { get; set; }

    /// <summary>
    ///  References the ID of the Section this Card belongs to.
    /// </summary>
    [Column("section_id")]
    public ulong SectionId { get; set; }

    /// <summary>
    ///  Enum containing the card's current status.
    /// </summary>
    /// <value></value>
    [Column("status")]
    public CardStatus Status { get; set; }

    /// <summary>
    ///  References the ID of the User who created this Card.
    ///  If it's null the user who created this card no longer exists.
    /// </summary>
    [Column("created_by")]
    public ulong? CreatedBy { get; set; }

    /// <summary>
    ///  Date and time this card was completed.
    /// </summary>
    [Column("completed_at")]
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    ///  The User assigned to this Card.
    /// </summary>
    public UserAccount? Assigned { get; set; }

    /// <summary>
    ///  The section that this card belongs to.
    /// </summary>
    public Section? Section { get; set; }

    /// <summary>
    ///  The User who created this Card.
    /// </summary>
    public UserAccount? Creator { get; set; }
}
