namespace Lattice.Domain.Entities;

/// <summary>
///  Class that abstracts common columns for entities in the database
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    ///  A 64-bit unsigned interger identifier for entities.
    /// </summary>
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    /// <summary>
    ///  Record of when a entitiy was created.
    /// </summary>
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}