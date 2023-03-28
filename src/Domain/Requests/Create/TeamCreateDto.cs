namespace Lattice.Domain.Dtos;

/// <summary>
///  Data transfer object used to create Team entities (Received in requests)
/// </summary>
public record struct TeamCreateDto
{
    /// <summary>
    ///  The User who owns this Team
    /// </summary>
    public ulong OwnerId { get; set; }

    /// <summary>
    ///  The display name of the Team
    /// </summary>
    public string Name { get; set; }
}