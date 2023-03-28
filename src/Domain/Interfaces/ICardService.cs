namespace Lattice.Domain.Interfaces;

public enum CardOperationResult
{
    Ok,
    NotFound,
    UserNotFound,
    UnknowError
}


public interface ICardService
{
    Task<(ulong?, CardOperationResult)> CreateAsync(CardCreateDto data);
    Task<CardOperationResult> UpdateAsync(ulong id, CardUpdateDto data);
    Task<CardOperationResult> DeleteAsync(ulong id);

    Task<CardOperationResult> AssignCardTo(ulong cardId, ulong userId);
    Task<CardDto?> GetCardInfo(ulong id);
}