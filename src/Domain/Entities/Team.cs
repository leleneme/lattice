namespace Lattice.Domain.Entities;

/// <summary>
///  A Team is a group of Users that can have multiple Boards.
/// </summary>
[Table("team")]
public class Team : BaseEntity
{
    /// <summary>
    ///  References the ID of the User who owns this Card.
    ///  It may not necessarily be who created it.
    /// </summary>
    [Column("owner_id")]
    public ulong OwnerId { get; set; }

    /// <summary>
    ///  Display name of the Team
    /// </summary>
    [Column("name")]
    public required string Name { get; set; } 

    /// <summary>
    ///  The ID of the User who owns this Card
    /// </summary>
    public UserAccount? Owner { get; set; }

    /// <summary>
    ///  List of Users that are on this Team.
    /// </summary>
    public List<UserTeam>? UserTeams { get; set; }

    /// <summary>
    ///  List of Boards this Team has.
    /// </summary>
    public List<Board>? Boards { get; set; }
}