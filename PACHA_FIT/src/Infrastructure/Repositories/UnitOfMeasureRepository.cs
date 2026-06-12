using Microsoft.EntityFrameworkCore;
using PACHA_FIT.Core.Domain.Inventory.Dtos;
using PACHA_FIT.Core.Domain.Inventory.Ports;
using PACHA_FIT.Infrastructure.Persistence;

namespace PACHA_FIT.Infrastructure.Repositories;

public class UnitOfMeasureRepository : IUnitOfMeasureRepository
{
    private readonly PachaFitContext _context;

    public UnitOfMeasureRepository(PachaFitContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UnitOfMeasureInfo>> GetAllAsync()
    {
        return await _context.UnitOfMeasures
            .AsNoTracking()
            .Select(u => new UnitOfMeasureInfo(u.Name, u.Abbreviation, u.Category, u.ConversionFactor, u.IsActive))
            .ToListAsync();
    }

    public async Task<IEnumerable<UnitOfMeasureInfo>> GetAllActiveAsync()
    {
        return await _context.UnitOfMeasures
            .AsNoTracking()
            .Where(u => u.IsActive)
            .Select(u => new UnitOfMeasureInfo(u.Name, u.Abbreviation, u.Category, u.ConversionFactor, u.IsActive))
            .ToListAsync();
    }
}
