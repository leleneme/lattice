namespace Lattice.Domain.Dtos;

/// <summary>
///  Data transfer object used to create User-Team memberships (Received in requests)
/// </summary>
/// <remarks>
///  Note that the Id of the Team is not contained in this type, since it is passed
///  by routing, and used to call directly ITeamService.AddMember(ulong teamId, UserJoinTeamDto data)
/// </remarks>
public record struct UserJoinTeamDto
{
    /// <summary>
    ///  The Id of the User to be added to a Team
    /// </summary>
    public ulong UserId { get; set; }
}