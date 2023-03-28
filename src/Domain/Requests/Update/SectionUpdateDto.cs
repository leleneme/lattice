namespace Lattice.Domain.Dtos;

/// <summary>
///  Data transfer object used to update Section entities (Received in requests)
/// </summary>
public record struct SectionUpdateDto
{
    /// <summary>
    ///  The display name of the Section
    /// </summary>
    public string Name { get; set; } 
}