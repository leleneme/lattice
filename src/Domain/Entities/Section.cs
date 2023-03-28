namespace Lattice.Domain.Entities;

/// <summary>
///  A Section belongs to a Board and it contains none, one or many Cards.
/// </summary>
[Table("section")]
public class Section : BaseEntity
{
    /// <summary>
    ///  References the ID of the Board this Section belongs to.
    /// </summary>
    [Column("board_id")]
    public ulong BoardId { get; set; }

    /// <summary>
    ///  Display name of the Section.
    /// </summary>
    [Column("name")]
    public required string Name { get; set; } 

    /// <summary>
    ///  The Board this Section belongs to.
    /// </summary>
    public Board? Board { get; set; }

    /// <summary>
    ///  List of Cards that this Section contains.
    /// </summary>
    public List<Card>? Cards { get; set; }
}
