namespace Lattice.Domain.Dtos;

/// <summary>
///  Data transfer object used to create Section entities (Received in requests)
/// </summary>
public record struct SectionCreateDto
{
    /// <summary>
    ///  The Board this Sections belongs to
    /// </summary>
    public ulong BoardId { get; set; }

    /// <summary>
    ///  The display name of the Section
    /// </summary>
    public string Name { get; set; } 
}