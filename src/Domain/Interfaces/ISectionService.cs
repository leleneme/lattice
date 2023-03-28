namespace Lattice.Domain.Interfaces;

public enum SectionOperationResult
{
    Ok,
    NotFound,
    UnknowError
}

public interface ISectionService
{
    Task<bool> ExistsAsync(ulong id);
    Task<(ulong?, SectionOperationResult)> CreateAsync(SectionCreateDto data);
    Task<SectionOperationResult> UpdateAsync(ulong id, SectionUpdateDto data);
    Task<SectionOperationResult> DeleteAsync(ulong id);

    Task<SectionDto?> GetSectionInfo(ulong id);
    Task<List<CardDto>?> GetSectionCards(ulong id);
}