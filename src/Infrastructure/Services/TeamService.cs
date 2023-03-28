using MapsterMapper;
using Lattice.Domain.Dtos;

namespace Lattice.Infrastructure.Services;

public class TeamService : ITeamService
{
    private readonly LatticeDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public TeamService(LatticeDbContext dbContext, IMapper mapper, IUserService userService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<(ulong?, TeamOperationResult)> CreateAsync(TeamCreateDto data)
    {
        if (!await _userService.ExistsAsync(data.OwnerId))
            return (null, TeamOperationResult.NotFound);

        var team = _mapper.Map<Team>(data);

        await _dbContext.Teams.AddAsync(team);

        return await _dbContext.SaveChangesAsync() > 0
            ? (team.Id, TeamOperationResult.Ok)
            : (null, TeamOperationResult.UnknowError);
    }

    public async Task<TeamOperationResult> DeleteAsync(ulong id)
    {
        var team = await _dbContext.Teams.FindAsync(id);

        if (team is null) return TeamOperationResult.NotFound;

        var teamMemberships = await _dbContext.UserTeams
            .Where(ut => ut.TeamId == id)
            .ToListAsync();

        _dbContext.UserTeams.RemoveRange(teamMemberships);
        _dbContext.Teams.Remove(team);

        return await _dbContext.SaveChangesAsync() > 0
            ? TeamOperationResult.Ok
            : TeamOperationResult.UnknowError;
    }

    public async Task<bool> ExistsAsync(ulong id)
    {
        var team = await _dbContext.Teams.FindAsync(id);
        return team is not null;
    }

    public async Task<List<TeamDto>> GetAllTeams()
    {
        var teams = await _dbContext.Teams
            .Include(t => t.UserTeams)
            .Include(t => t.Owner)
            .ToListAsync();

        return _mapper.Map<List<TeamDto>>(teams);
    }

    public async Task<List<BoardDto>?> GetTeamBoards(ulong id)
    {
        bool exists = await this.ExistsAsync(id);

        if (!exists) return null;

        var boards = await _dbContext.Boards
            .Where(b => b.TeamId == id)
            .Include(b => b.Creator)
            .Include(b => b.Sections)
            .ToListAsync();

        return _mapper.Map<List<BoardDto>>(boards);
    }

    public async Task<TeamDto?> GetTeamInfo(ulong id)
    {
        var team = await _dbContext.Teams
            .Include(t => t.UserTeams!)
                .ThenInclude(ut => ut.User)
            .Where(t => t.Id == id)
            .FirstOrDefaultAsync();

        if (team is null) return null;

        return team is not null
            ? _mapper.Map<TeamDto>(team) : null;
    }

    public async Task<List<UserDto>?> GetTeamMembers(ulong id)
    {
        var members = await _dbContext.UserTeams
            .Include(ut => ut.User)
            .ToListAsync();

        return _mapper.Map<List<UserDto>>(members);
    }

    public async Task<TeamOperationResult> UpdateAsync(ulong id, TeamUpdateDto data)
    {
        var team = await _dbContext.Teams.FindAsync(id);
        
        if (team is null) return TeamOperationResult.NotFound;

        if (team.Name == data.Name) return TeamOperationResult.Ok;

        team.Name = data.Name;

        return await _dbContext.SaveChangesAsync() > 0
            ? TeamOperationResult.Ok
            : TeamOperationResult.UnknowError;
    }

    public async Task<TeamOperationResult> AddMember(ulong teamId, UserJoinTeamDto data)
    {
        bool userExists = await _userService.ExistsAsync(data.UserId);
        bool teamExists = await this.ExistsAsync(teamId);

        if (!userExists || !teamExists)
            return TeamOperationResult.NotFound;

        UserTeam membership = new UserTeam { UserId = data.UserId, TeamId = teamId };

        await _dbContext.UserTeams.AddAsync(membership);

        return await _dbContext.SaveChangesAsync() > 0
            ? TeamOperationResult.Ok
            : TeamOperationResult.UnknowError;
    }

    public async Task<TeamOperationResult> RemoveMember(ulong teamId, ulong userId)
    {
        UserTeam? membership = await _dbContext.UserTeams
            .Where(ut => ut.UserId == userId && ut.TeamId == teamId)
            .FirstOrDefaultAsync();

        if (membership is null) return TeamOperationResult.NotFound;

        _dbContext.UserTeams.Remove(membership);

        return await _dbContext.SaveChangesAsync() > 0
            ? TeamOperationResult.Ok
            : TeamOperationResult.UnknowError;
    }
}