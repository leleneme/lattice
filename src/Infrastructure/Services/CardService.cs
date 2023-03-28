using MapsterMapper;
using Lattice.Domain.Dtos;

namespace Lattice.Infrastructure.Services;

public class CardService : ICardService
{
    private readonly LatticeDbContext _dbContext;
    private readonly IUserService _userService;
    private readonly ISectionService _sectionService;
    private readonly IMapper _mapper;

    public CardService(
        LatticeDbContext dbContext, IUserService userService,
        IMapper mapper, ISectionService sectionService)
    {
        _dbContext = dbContext;
        _userService = userService;
        _mapper = mapper;
        _sectionService = sectionService;
    }

    public async Task<CardOperationResult> AssignCardTo(ulong cardId, ulong userId)
    {
        var card = await _dbContext.Cards.FindAsync(cardId);

        if (card is null)
            return CardOperationResult.NotFound;

        if (!await _userService.ExistsAsync(userId))
            return CardOperationResult.UserNotFound;

        card.AssignedTo = userId;

        return await _dbContext.SaveChangesAsync() > 0
            ? CardOperationResult.Ok
            : CardOperationResult.UnknowError;
    }

    public async Task<(ulong?, CardOperationResult)> CreateAsync(CardCreateDto data)
    {
        bool sectionExists = await _sectionService.ExistsAsync(data.SectionId);

        if (!sectionExists) return (null, CardOperationResult.NotFound);

        var card = _mapper.Map<Card>(data);
        card.CreatedAt = DateTime.UtcNow;

        await _dbContext.Cards.AddAsync(card);

        return await _dbContext.SaveChangesAsync() > 0
            ? (card.Id, CardOperationResult.Ok)
            : (null, CardOperationResult.UnknowError);
    }

    public async Task<CardOperationResult> DeleteAsync(ulong id)
    {
        var card = await _dbContext.Cards.FindAsync(id);

        if (card is null) return CardOperationResult.NotFound;

        _dbContext.Cards.Remove(card);

        return await _dbContext.SaveChangesAsync() > 0
            ? CardOperationResult.Ok
            : CardOperationResult.UnknowError;
    }

    public async Task<CardDto?> GetCardInfo(ulong id)
    {
        var card = await _dbContext.Cards.FindAsync(id);

        return card is not null
            ? _mapper.Map<CardDto>(card) : null;
    }

    public async Task<CardOperationResult> UpdateAsync(ulong id, CardUpdateDto data)
    {
        var card = await _dbContext.Cards.FindAsync(id);

        if (card is null) return CardOperationResult.NotFound;

        card.SectionId = data.SectionId;
        card.Name = data.Name;
        card.Description = data.Description;
        card.Status = data.Status;

        _dbContext.Entry(card).CurrentValues.SetValues(card);

        return await _dbContext.SaveChangesAsync() > 0
            ? CardOperationResult.Ok
            : CardOperationResult.UnknowError;
    }
}