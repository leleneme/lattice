namespace Lattice.Domain.Dtos;

/// <summary>
///  Data transfer object representing a Section, to be sent by Controllers in responses
/// </summary>
public struct SectionDto
{
    /// <summary>
    ///  The Id of the Section
    /// </summary>
    public ulong Id { get; set; }

    /// <summary>
    ///  The DateTime this Section was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///  The Board this Section belongs to
    /// </summary>
    public ulong BoardId { get; set; }

    /// <summary>
    ///  The display name of the Section
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///  List of Cards contained in this Section
    /// </summary>
    public List<CardDto>? Cards { get; set; }
}