namespace Lattice.Domain.Dtos;

/// <summary>
///  Data transfer object used to update Board entities (Received in requests)
/// </summary>
public record struct BoardUpdateDto
{
    /// <summary>
    ///  The name of the Board
    /// </summary>
    public string Name { get; set; }
}