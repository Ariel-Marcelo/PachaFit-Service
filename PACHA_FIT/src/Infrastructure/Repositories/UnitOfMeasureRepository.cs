using Microsoft.EntityFrameworkCore;
using PACHA_FIT.Core.Domain.Inventory.Ports;
using PACHA_FIT.Infrastructure.Persistence;
using PACHA_FIT.Infrastructure.Persistence.Entities;

namespace PACHA_FIT.Infrastructure.Repositories;

public class UnitOfMeasureRepository : IUnitOfMeasureRepository
{
    private readonly PachaFitContext _context;

    public UnitOfMeasureRepository(PachaFitContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UnitOfMeasure>> GetAllAsync()
    {
        return await _context.UnitOfMeasures
            .AsNoTracking()
            .ToListAsync();
    }
}
