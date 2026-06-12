using PACHA_FIT.Core.Domain.Inventory.Dtos;
using PACHA_FIT.Core.Domain.Inventory.Ports;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;

namespace PACHA_FIT.Core.Application.Inventory;

public class UnitOfMeasureService
{
    private readonly IUnitOfMeasureRepository _repository;
    private Dictionary<string, UnitOfMeasureInfo>? _cache;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public UnitOfMeasureService(IUnitOfMeasureRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<UnitOfMeasureGroupedDto>> GetAllActiveUnitsAsync()
    {
        var cache = await GetCacheAsync();
        var groupedDto = UnitOfMeasureGroupedDto.CreateFromEntities(cache.Values);
        return Result<UnitOfMeasureGroupedDto>.Success(groupedDto);
    }

    public async Task<Result<decimal>> GetConversionFactor(string abbreviation)
    {
        var cache = await GetCacheAsync();
        if (cache.TryGetValue(abbreviation, out var unit))
        {
            return Result<decimal>.Success(unit.ConversionFactor);
        }

        return Result<decimal>.Failure($"Unknown unit of measure: {abbreviation}", ErrorType.NotFound);
    }

    public async Task<Result<decimal>> Convert(decimal quantity, string fromUnit, string toUnit)
    {
        var cache = await GetCacheAsync();
        
        if (!cache.TryGetValue(fromUnit, out var from))
            return Result<decimal>.Failure($"Unknown unit of measure: {fromUnit}", ErrorType.NotFound);
            
        if (!cache.TryGetValue(toUnit, out var to))
            return Result<decimal>.Failure($"Unknown unit of measure: {toUnit}", ErrorType.NotFound);

        if (from.Category != to.Category)
        {
            return Result<decimal>.Failure($"Incompatibilidad de unidades: no se puede convertir {from.Category} a {to.Category}", ErrorType.Validation);
        }

        var result = (quantity * from.ConversionFactor) / to.ConversionFactor;
        return Result<decimal>.Success(result);
    }

    public async Task<Result<string>> GetUnitCategory(string abbreviation)
    {
        var cache = await GetCacheAsync();
        if (cache.TryGetValue(abbreviation, out var unit))
        {
            return Result<string>.Success(unit.Category);
        }
        return Result<string>.Failure($"Unknown unit of measure: {abbreviation}", ErrorType.NotFound);
    }
    
    private async Task<Dictionary<string, UnitOfMeasureInfo>> GetCacheAsync()
    {
        if (_cache != null) return _cache;

        await _semaphore.WaitAsync();
        try
        {
            if (_cache == null)
            {
                var units = await _repository.GetAllActiveAsync();
                _cache = units.ToDictionary(u => u.Abbreviation, u => u);
            }
            return _cache;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void ClearCache()
    {
        _cache = null;
    }
}
