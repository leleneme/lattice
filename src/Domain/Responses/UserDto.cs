namespace Lattice.Domain.Dtos;

/// <summary>
///  Data transfer object representing a User Account, to be sent by Controllers in responses
/// </summary>
public struct UserDto
{
    /// <summary>
    ///  The Id of the User
    /// </summary>
    public ulong Id { get; set; }

    /// <summary>
    ///  The DateTime this User was registered
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///  The name of the User, can be his full name or a username
    /// </summary>
    public string Name { get; set; }
}