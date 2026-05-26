using PACHA_FIT.Infrastructure.Persistence.Entities;

namespace PACHA_FIT.Core.Domain.Inventory.Ports;

public interface IUnitOfMeasureRepository
{
    Task<IEnumerable<UnitOfMeasure>> GetAllAsync();
}
