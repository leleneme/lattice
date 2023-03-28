namespace Lattice.Domain.Interfaces;

public enum BoardOperationResult
{
    Ok,
    NotFound,
    UnknowError
}


public interface IBoardService
{
    Task<bool> ExistsAsync(ulong id);
    Task<(ulong?, BoardOperationResult)> CreateAsync(BoardCreateDto data);
    Task<BoardOperationResult> UpdateAsync(ulong id, BoardUpdateDto data);
    Task<BoardOperationResult> DeleteAsync(ulong id);

    Task<BoardDto?> GetBoardInfo(ulong id);
}