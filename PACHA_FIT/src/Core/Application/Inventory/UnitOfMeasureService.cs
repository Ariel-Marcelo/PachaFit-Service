using PACHA_FIT.Core.Domain.Inventory.Ports;
using PACHA_FIT.Infrastructure.Persistence.Entities;

namespace PACHA_FIT.Core.Application.Inventory;

public class UnitOfMeasureService
{
    private readonly IUnitOfMeasureRepository _repository;
    private Dictionary<string, UnitOfMeasure>? _cache;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public UnitOfMeasureService(IUnitOfMeasureRepository repository)
    {
        _repository = repository;
    }

    private async Task<Dictionary<string, UnitOfMeasure>> GetCacheAsync()
    {
        if (_cache != null) return _cache;

        await _semaphore.WaitAsync();
        try
        {
            if (_cache == null)
            {
                var units = await _repository.GetAllAsync();
                _cache = units.ToDictionary(u => u.Abbreviation, u => u);
            }
            return _cache;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<decimal> GetConversionFactor(string abbreviation)
    {
        var cache = await GetCacheAsync();
        if (cache.TryGetValue(abbreviation, out var unit))
        {
            return unit.ConversionFactor;
        }

        throw new ArgumentException($"Unknown unit of measure: {abbreviation}");
    }

    public async Task<decimal> Convert(decimal quantity, string fromUnit, string toUnit)
    {
        var cache = await GetCacheAsync();
        
        if (!cache.TryGetValue(fromUnit, out var from))
            throw new ArgumentException($"Unknown unit of measure: {fromUnit}");
            
        if (!cache.TryGetValue(toUnit, out var to))
            throw new ArgumentException($"Unknown unit of measure: {toUnit}");

        if (from.Category != to.Category)
        {
            throw new InvalidOperationException($"Incompatibilidad de unidades: no se puede convertir {from.Category} a {to.Category}");
        }

        return (quantity * from.ConversionFactor) / to.ConversionFactor;
    }

    public async Task<string> GetUnitCategory(string abbreviation)
    {
        var cache = await GetCacheAsync();
        if (cache.TryGetValue(abbreviation, out var unit))
        {
            return unit.Category;
        }
        throw new ArgumentException($"Unknown unit of measure: {abbreviation}");
    }
}
