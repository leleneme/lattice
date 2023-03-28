namespace Lattice.Domain.Dtos;

/// <summary>
///  Response type contained the Id of a newly created entity
/// </summary>
public record struct CreationResult
{
    public ulong Id { get; set; }

    public CreationResult(ulong id) 
    {
        this.Id = id;
    }

    public CreationResult(ulong? id)
    {
        this.Id = (ulong)id!;
    }
};