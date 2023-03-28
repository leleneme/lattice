namespace Lattice.Domain.Dtos;

/// <summary>
///  Data transfer object used to update User entities (Received in requests)
/// </summary>
public record struct UserUpdateDto
{
    /// <summary>
    ///  The name of the User, can be his full name or a username
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///  The new plain-text password to be used in hashing
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    ///  The E-mail of the Usar
    /// </summary>
    public string Email { get; set; }
}