using PACHA_FIT.Core.Domain.Inventory.Dtos;

namespace PACHA_FIT.Core.Domain.Inventory.Ports;

public interface IStockMovementRepository
{
    Task SaveMovement(StockMovementRequest request);
    Task<IEnumerable<StockBatchResponse>> GetAvailableBatches(int productId);
}
