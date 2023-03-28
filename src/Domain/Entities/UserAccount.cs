using System.Text.Json.Serialization;

namespace Lattice.Domain.Entities;

/// <summary>
///  Entity that represents a User of the system.
/// </summary>
[Table("user_account")]
public class UserAccount : BaseEntity
{
    /// <summary>
    ///  Display name of the User, can be a them name or any valid username.
    /// </summary>
    [Column("name")]
    public string Name { get; set; } = "";

    /// <summary>
    ///  Encrypted User password.
    /// </summary>
    [Column("password_hash")]
    public string PasswordHash { get; set; } = "";

    /// <summary>
    ///  E-mail of this User.
    /// </summary>
    [Column("email")]
    public string Email { get; set; } = "";

    /// <summary>
    ///  List of Teams this User is member of.
    /// </summary>
    public List<UserTeam>? UserTeams { get; set; }

    /// <summary>
    ///  List of Cards this User is assigned to
    /// </summary>
    /// <value></value>
    public List<Card>? Cards { get; set; }
}