using PACHA_FIT.Core.Domain.Inventory.Dtos;
using PACHA_FIT.Core.Domain.Inventory.Ports;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;

namespace PACHA_FIT.Core.Application.Inventory;

public class InventoryService
{
    private readonly IStockMovementRepository _stockMovementRepository;

    public InventoryService(IStockMovementRepository stockMovementRepository)
    {
        _stockMovementRepository = stockMovementRepository;
    }

    public async Task<Result<string>> DispatchStock(int productId, decimal quantity, string unitAbbreviation)
    {
        var batches = (await _stockMovementRepository.GetAvailableBatches(productId))
            .OrderBy(b => b.ExpiryDate ?? DateTime.MaxValue)
            .ToList();

        decimal totalAvailable = batches.Sum(b => b.AvailableQty);
        if (totalAvailable < quantity)
        {
            return Result<string>.Failure(new Error(SystemError.Validation, "Stock insuficiente para completar el despacho"));
        }

        decimal remainingToDispatch = quantity;

        foreach (var batch in batches)
        {
            if (remainingToDispatch <= 0) break;

            decimal dispatchFromThisBatch = Math.Min(batch.AvailableQty, remainingToDispatch);

            await _stockMovementRepository.SaveMovement(new StockMovementRequest(
                ProductId: productId,
                InputQuantity: dispatchFromThisBatch,
                InputUnitAbbreviation: unitAbbreviation,
                BaseQuantityAffected: dispatchFromThisBatch, // Simplification
                TypeMovement: "Egreso",
                ExpiryDate: batch.ExpiryDate,
                Description: "Despacho por venta (FEFO)"
            ));

            remainingToDispatch -= dispatchFromThisBatch;
        }

        return Result<string>.Success("Despacho completado exitosamente");
    }
}
