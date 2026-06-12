namespace PACHA_FIT.Core.Domain.Inventory.Dtos;

public record StockMovementRequest(
    int ProductId,
    decimal InputQuantity,
    string InputUnitAbbreviation,
    decimal BaseQuantityAffected,
    string TypeMovement,
    int? AdjustmentReasonId = null,
    DateTime? ExpiryDate = null,
    string? Description = null
);

public record StockBatchResponse(
    int ProductId,
    decimal AvailableQty,
    DateTime? ExpiryDate
);
