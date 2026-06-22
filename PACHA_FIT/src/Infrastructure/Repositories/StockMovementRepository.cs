using Microsoft.EntityFrameworkCore;
using PACHA_FIT.Core.Domain.Inventory.Dtos;
using PACHA_FIT.Core.Domain.Inventory.Ports;
using PACHA_FIT.Infrastructure.Persistence;
using PACHA_FIT.Infrastructure.Persistence.Entities;

namespace PACHA_FIT.Infrastructure.Repositories;

public class StockMovementRepository : IStockMovementRepository
{
    private readonly PachaFitContext _context;

    public StockMovementRepository(PachaFitContext context)
    {
        _context = context;
    }

    public async Task SaveMovement(StockMovementRequest request)
    {
        var unit = await _context.UnitOfMeasures.FirstOrDefaultAsync(u => u.Abbreviation == request.InputUnitAbbreviation);
        
        var movement = new StockMovement
        {
            ProductId = request.ProductId,
            InputQuantity = request.InputQuantity,
            InputUnitId = unit?.UnitId,
            BaseQuantityAffected = request.BaseQuantityAffected,
            TypeMovement = request.TypeMovement,
            AdjustmentReasonId = request.AdjustmentReasonId,
            ExpiryDate = request.ExpiryDate,
            // Description = request.Description, // Entity has no Description field
            CreatedAt = DateTime.UtcNow
        };

        _context.StockMovements.Add(movement);
        
        // Update product stock quantity
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == request.ProductId);
        if (product != null)
        {
            if (request.TypeMovement == "Ingreso")
            {
                product.StockQty = (product.StockQty ?? 0) + request.BaseQuantityAffected;
            }
            else if (request.TypeMovement == "Egreso")
            {
                product.StockQty = (product.StockQty ?? 0) - request.BaseQuantityAffected;
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<StockBatchResponse>> GetAvailableBatches(int productId)
    {
        return await _context.StockMovements
            .AsNoTracking()
            .Where(m => m.ProductId == productId && m.TypeMovement == "Ingreso" && m.BaseQuantityAffected > 0)
            .Select(m => new StockBatchResponse(m.ProductId ?? 0, m.BaseQuantityAffected, m.ExpiryDate))
            .ToListAsync();
    }
}
