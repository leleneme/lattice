namespace Lattice.Domain.Dtos;

/// <summary>
///  Data transfer object used to create Board entities (Received in requests)
/// </summary>
public record struct BoardCreateDto
{
    /// <summary>
    ///  The Team this Board belongs to
    /// </summary>
    public ulong TeamId { get; set; }

    /// <summary>
    ///  The display name of the Board
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///  The UserId of whoever created this Board, must be valid on creation
    /// </summary>
    public ulong CreatedBy { get; set; }
}