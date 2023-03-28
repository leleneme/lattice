using MapsterMapper;
using Lattice.Domain.Dtos;

namespace Lattice.Infrastructure.Services;

public class SectionService : ISectionService
{
    private readonly LatticeDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IBoardService _boardService;

    public SectionService(LatticeDbContext dbContext, IMapper mapper, IBoardService boardService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _boardService = boardService;
    }

    public async Task<bool> ExistsAsync(ulong id)
    {
        var section = await _dbContext.Sections.FindAsync(id);
        return section is not null;
    }

    public async Task<(ulong?, SectionOperationResult)> CreateAsync(SectionCreateDto data)
    {
        if (!await _boardService.ExistsAsync(data.BoardId))
            return (null, SectionOperationResult.NotFound);

        var section = _mapper.Map<Section>(data);

        await _dbContext.Sections.AddAsync(section);

        return await _dbContext.SaveChangesAsync() > 0
            ? (section.Id, SectionOperationResult.Ok)
            : (null, SectionOperationResult.UnknowError);
    }

    public async Task<SectionOperationResult> DeleteAsync(ulong id)
    {
        var section = await _dbContext.Sections.FindAsync(id);

        if (section is null)
            return SectionOperationResult.NotFound;

        _dbContext.Sections.Remove(section);

        return await _dbContext.SaveChangesAsync() > 0
            ? SectionOperationResult.Ok
            : SectionOperationResult.UnknowError;
    }

    public async Task<List<CardDto>?> GetSectionCards(ulong id)
    {
        bool exists = await this.ExistsAsync(id);

        if (!exists) return null;

        var cards = await _dbContext.Sections
            .Include(s => s.Cards!)
                .ThenInclude(c => c.Creator)
            .Include(s => s.Cards!)
                .ThenInclude(c => c.Assigned)
            .Where(s => s.Id == id)
            .ToListAsync();

        return _mapper.Map<List<CardDto>>(cards);
    }

    public async Task<SectionDto?> GetSectionInfo(ulong id)
    {
        var section = await _dbContext.Sections
            .Include(s => s.Board)
            .Include(s => s.Cards!)
                .ThenInclude(c => c.Creator)
            .Include(s => s.Cards!)
                .ThenInclude(c => c.Assigned)
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();

        return section is not null
            ? _mapper.Map<SectionDto>(section) : null;
    }

    public async Task<SectionOperationResult> UpdateAsync(ulong id, SectionUpdateDto data)
    {
        var section = await _dbContext.Sections.FindAsync(id);

        if (section is null)
            return SectionOperationResult.NotFound;

        if (section.Name == data.Name) return SectionOperationResult.Ok;

        section.Name = data.Name;

        return await _dbContext.SaveChangesAsync() > 0
            ? SectionOperationResult.Ok
            : SectionOperationResult.UnknowError;
    }
}