using Mapster;
using MapsterMapper;
using Lattice.Domain.Dtos;

namespace Lattice.Infrastructure.Services;

public class BoardService : IBoardService
{
    private readonly LatticeDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ITeamService _teamService;

    public BoardService(LatticeDbContext dbContext, IMapper mapper, ITeamService teamService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _teamService = teamService;
    }

    public async Task<bool> ExistsAsync(ulong id)
    {
        var board = await _dbContext.Boards.FindAsync(id);
        return board is not null;
    }

    public async Task<(ulong?, BoardOperationResult)> CreateAsync(BoardCreateDto data)
    {
        if (!await _teamService.ExistsAsync(data.TeamId))
            return (null, BoardOperationResult.NotFound);

        var board = _mapper.Map<Board>(data);

        await _dbContext.Boards.AddAsync(board);

        return await _dbContext.SaveChangesAsync() > 0
            ? (board.Id, BoardOperationResult.Ok)
            : (null, BoardOperationResult.UnknowError);
    }

    public async Task<BoardOperationResult> DeleteAsync(ulong id)
    {
        var board = await _dbContext.Boards.FindAsync(id);

        if (board is null)
            return BoardOperationResult.NotFound;

        _dbContext.Boards.Remove(board);

        return await _dbContext.SaveChangesAsync() > 0
            ? BoardOperationResult.Ok
            : BoardOperationResult.UnknowError;
    }

    public async Task<BoardDto?> GetBoardInfo(ulong id)
    {
        var board = await _dbContext.Boards
            .Where(b => b.Id == id)
            .Include(b => b.Creator)
            .Include(b => b.Sections)
            .FirstOrDefaultAsync();

        if (board is null) return null;

        return _mapper.Map<BoardDto>(board);
    }

    public async Task<BoardOperationResult> UpdateAsync(ulong id, BoardUpdateDto data)
    {
        var board = await _dbContext.Boards.FindAsync(id);

        if (board is null) return BoardOperationResult.NotFound;

        if (board.Name == data.Name) return BoardOperationResult.Ok;

        board.Name = data.Name;

        return await _dbContext.SaveChangesAsync() > 0
            ? BoardOperationResult.Ok
            : BoardOperationResult.UnknowError;
    }
}