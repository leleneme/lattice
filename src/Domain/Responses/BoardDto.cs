namespace Lattice.Domain.Dtos;

/// <summary>
///  Data transfer object representing a Board, to be sent by Controllers in responses
/// </summary>
public struct BoardDto
{
    /// <summary>
    ///  The Id of the Board
    /// </summary>
    public ulong Id { get; set; }

    /// <summary>
    ///  The DateTime this Board was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///  The Id of the Team this Board belongs to
    /// </summary>
    public ulong TeamId { get; set; }

    /// <summary>
    ///  The display name of the Board
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///  The User who created this Board
    /// </summary>
    public UserDto? Creator { get; set; }

    /// <summary>
    ///  List of Sections this Board contains
    /// </summary>
    public List<SectionDto> Sections { get; set; }
}