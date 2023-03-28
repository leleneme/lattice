namespace Lattice.Domain.Entities;

/// <summary>
///  Entity that represents membership of a User in a Team.
/// </summary>
[Table("user_team")]
public class UserTeam : BaseEntity
{
    /// <summary>
    ///  References the ID of the Team.
    /// </summary>
    [Column("team_id")]
    public ulong TeamId { get; set; }

    /// <summary>
    ///  References the ID of the User.
    /// </summary>
    [Column("user_id")]
    public ulong UserId { get; set; }

    public UserAccount? User { get; set; }
    public Team? Team { get; set; }
}