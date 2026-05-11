namespace PACHA_FIT.Core.Application.Inventory;

public class AdjustmentReasons
{
    private static readonly HashSet<string> PredefinedReasons = new()
    {
        "Caducidad",
        "Rotura",
        "Consumo Interno",
        "Error de Inventario"
    };

    public bool IsValidReason(string reason)
    {
        return PredefinedReasons.Contains(reason);
    }

    public IEnumerable<string> GetAllReasons()
    {
        return PredefinedReasons;
    }
}