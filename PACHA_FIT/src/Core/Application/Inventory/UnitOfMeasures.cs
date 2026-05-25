namespace PACHA_FIT.Core.Application.Inventory;

public class UnitOfMeasures
{
    private static readonly Dictionary<string, decimal> MassFactors = new()
    {
        { "g", 1.0m },
        { "lb", 454.0m },
        { "@", 11350.0m },
        { "qq", 45400.0m },
        { "kg", 1000.0m }
    };

    private static readonly Dictionary<string, decimal> VolumeFactors = new()
    {
        { "ml", 1.0m },
        { "L", 1000.0m }
    };

    private static readonly Dictionary<string, decimal> DiscreteFactors = new()
    {
        { "u", 1.0m }
    };

    public decimal GetConversionFactor(string abbreviation)
    {
        if (MassFactors.TryGetValue(abbreviation, out var massFactor)) return massFactor;
        if (VolumeFactors.TryGetValue(abbreviation, out var volumeFactor)) return volumeFactor;
        if (DiscreteFactors.TryGetValue(abbreviation, out var discreteFactor)) return discreteFactor;

        throw new ArgumentException($"Unknown unit of measure: {abbreviation}");
    }

    public decimal Convert(decimal quantity, string fromUnit, string toUnit)
    {
        var fromCategory = GetUnitCategory(fromUnit);
        var toCategory = GetUnitCategory(toUnit);

        if (fromCategory != toCategory)
        {
            throw new InvalidOperationException($"Incompatibilidad de unidades: no se puede convertir {fromCategory} a {toCategory}");
        }

        var fromFactor = GetConversionFactor(fromUnit);
        var toFactor = GetConversionFactor(toUnit);

        return (quantity * fromFactor) / toFactor;
    }

    private string GetUnitCategory(string abbreviation)
    {
        if (MassFactors.ContainsKey(abbreviation)) return "masa";
        if (VolumeFactors.ContainsKey(abbreviation)) return "volumen";
        if (DiscreteFactors.ContainsKey(abbreviation)) return "unidades";
        throw new ArgumentException($"Unknown unit of measure: {abbreviation}");
    }
}