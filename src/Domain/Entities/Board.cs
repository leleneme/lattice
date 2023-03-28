namespace Lattice.Domain.Entities;

/// <summary>
///  A board is a entity that belongs to a Team and that may contains multiple Sections and Tasks
/// </summary>
[Table("board")]
public class Board : BaseEntity
{
    /// <summary>
    ///  References the ID of the User who created this board.
    ///  If it's null the user who created this card no longer exists.
    /// </summary>
    [Column("created_by")]
    public ulong? CreatedBy { get; set; }

    /// <summary>
    ///  References the ID of the team this board belongs to.
    /// </summary>
    [Column("team_id")]
    public ulong TeamId { get; set; }

    /// <summary>
    ///  Display name of the board.
    /// </summary>
    [Column("name")]
    public required string Name { get; set; } 

    /// <summary>
    ///  The team this board belongs to.
    /// </summary>
    public Team? Team { get; set; }

    /// <summary>
    ///  The User who created this board.
    /// </summary>
    public UserAccount? Creator { get; set; }

    /// <summary>
    ///  List of sections in this board.
    /// </summary>
    public List<Section>? Sections { get; set; }
}
