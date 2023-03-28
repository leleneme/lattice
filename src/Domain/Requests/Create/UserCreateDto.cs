namespace Lattice.Domain.Dtos;

/// <summary>
///  Data transfer object used to create User entities (Received in requests)
/// </summary>
public record struct UserCreateDto
{
    /// <summary>
    ///  The name of the User, can be his full name or a username
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///  The plain-text password to be used in hashing
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    ///  The E-mail of the User
    /// </summary>
    public string Email { get; set; }
}