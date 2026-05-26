using PACHA_FIT.Core.Domain.Inventory.Dtos;

namespace PACHA_FIT.Core.Domain.Inventory.Ports;

public interface IUnitOfMeasureRepository
{
    Task<IEnumerable<UnitOfMeasureInfo>> GetAllAsync();
    Task<IEnumerable<UnitOfMeasureInfo>> GetAllActiveAsync();
}
