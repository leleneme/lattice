namespace Lattice.Domain.Interfaces;

public enum UserOperationResult
{
    Ok,
    NotFound,
    EmailAlreadyTaken,
    InvalidEmail,
    InvalidPassword,
    UnknowError,
}

public interface IUserService
{
    Task<bool> ExistsAsync(ulong id);
    Task<(ulong?, UserOperationResult)> CreateAsync(UserCreateDto data);
    Task<UserOperationResult> UpdateAsync(ulong id, UserUpdateDto data);
    Task<UserOperationResult> DeleteAsync(ulong id);

    Task<UserDto?> GetUserInfo(ulong id);
    Task<List<UserDto>> GetAllUsers();
    Task<List<TeamDto>?> GetUserTeams(ulong id);
    Task<List<CardDto>?> GetUserAssignedCards(ulong id);
}