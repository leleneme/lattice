using MapsterMapper;
using Lattice.Domain.Dtos;
using BC = BCrypt.Net.BCrypt;
using System.ComponentModel.DataAnnotations;

namespace Lattice.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly LatticeDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserService(LatticeDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    private bool IsEmailValid(string email)
    {
        var emailAttribute = new EmailAddressAttribute();
        return emailAttribute.IsValid(email);
    }

    public async Task<bool> ExistsAsync(ulong id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        return user is not null;
    }

    public async Task<(ulong?, UserOperationResult)> CreateAsync(UserCreateDto data)
    {
        if (!IsEmailValid(data.Email))
            return (null, UserOperationResult.InvalidEmail);

        bool userExists = await _dbContext.Users
            .Where(u => u.Email == data.Email)
            .FirstOrDefaultAsync() is not null ? true : false;

        if (userExists)
            return (null, UserOperationResult.EmailAlreadyTaken);

        UserAccount user = _mapper.Map<UserAccount>(data);
        user.PasswordHash = BC.HashPassword(user.PasswordHash);

        await _dbContext.Users.AddAsync(user);

        return await _dbContext.SaveChangesAsync() > 0
            ? (user.Id, UserOperationResult.Ok)
            : (null, UserOperationResult.UnknowError);
    }

    public async Task<UserOperationResult> DeleteAsync(ulong id)
    {
        UserAccount? user = await _dbContext.Users.FindAsync(id);

        if (user is null)
            return UserOperationResult.NotFound;

        var teamMemberships = await _dbContext.UserTeams
            .Where(ut => ut.UserId == id)
            .ToListAsync();

        _dbContext.UserTeams.RemoveRange(teamMemberships);
        _dbContext.Users.Remove(user);

        return await _dbContext.SaveChangesAsync() > 0
            ? UserOperationResult.Ok
            : UserOperationResult.UnknowError;
    }

    public async Task<UserOperationResult> UpdateAsync(ulong id, UserUpdateDto data)
    {
        UserAccount? user = await _dbContext.Users.FindAsync(id);

        if (user is null)
            return UserOperationResult.NotFound;

        if (!IsEmailValid(data.Email)) return UserOperationResult.InvalidEmail;

        user.Name = data.Name;
        user.PasswordHash = BC.HashPassword(data.Password);
        user.Email = data.Email;

        _dbContext.Entry(user).CurrentValues.SetValues(user);

        return await _dbContext.SaveChangesAsync() > 0
            ? UserOperationResult.Ok
            : UserOperationResult.UnknowError;
    }

    public async Task<List<UserDto>> GetAllUsers()
    {
        var users = await _dbContext.Users.ToListAsync();
        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<List<CardDto>?> GetUserAssignedCards(ulong id)
    {
        bool exists = await this.ExistsAsync(id);

        if (!exists) return null;

        List<Card> cards = await _dbContext.Cards
            .Where(c => c.AssignedTo == id)
            .ToListAsync();

        return _mapper.Map<List<CardDto>>(cards);
    }

    public async Task<UserDto?> GetUserInfo(ulong id)
    {
        UserAccount? user = await _dbContext.Users
            .Include(u => u.UserTeams!)
            .ThenInclude(ut => ut.Team)
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();

        if (user is null) return null;

        return _mapper.Map<UserDto>(user);
    }

    public async Task<List<TeamDto>?> GetUserTeams(ulong id)
    {
        bool exists = await this.ExistsAsync(id);

        if (!exists) return null;

        List<UserTeam> userTeams = await _dbContext.UserTeams
            .Where(ut => ut.UserId == id)
            .Include(ut => ut.Team)
            .ThenInclude(ut => ut!.Owner)
            .ToListAsync();

        return _mapper.Map<List<TeamDto>>(userTeams);
    }
}