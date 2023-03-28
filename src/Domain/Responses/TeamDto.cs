namespace Lattice.Domain.Dtos;

/// <summary>
///  Data transfer object representing a Team, to be sent by Controllers in responses
/// </summary>
public struct TeamDto
{
    /// <summary>
    ///  The Id of the Team
    /// </summary>
    public ulong Id { get; set; }

    /// <summary>
    ///  The DateTime this Team was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///  The display name of the Team
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///  The User who owns this Team
    /// </summary>
    public UserDto Owner { get; set; }

    /// <summary>
    ///  List of members of this Team
    /// </summary>
    public List<UserDto> Members { get; set; }
}