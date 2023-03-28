namespace Lattice.Domain.Interfaces;

public enum TeamOperationResult
{
    Ok,
    NotFound,
    UnknowError
}

public interface ITeamService
{
    Task<bool> ExistsAsync(ulong id);
    Task<(ulong?, TeamOperationResult)> CreateAsync(TeamCreateDto data);
    Task<TeamOperationResult> UpdateAsync(ulong id, TeamUpdateDto data);
    Task<TeamOperationResult> DeleteAsync(ulong id);

    Task<TeamDto?> GetTeamInfo(ulong id);
    Task<List<TeamDto>> GetAllTeams();
    Task<List<BoardDto>?> GetTeamBoards(ulong id);

    Task<List<UserDto>?> GetTeamMembers(ulong id);
    Task<TeamOperationResult> AddMember(ulong teamId, UserJoinTeamDto data);
    Task<TeamOperationResult> RemoveMember(ulong teamId, ulong userId);
}