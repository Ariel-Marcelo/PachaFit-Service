namespace PACHA_FIT.Core.Domain.Inventory.Dtos;

public record UnitOfMeasureItemDto(string Abbreviation, string Name);

public record UnitOfMeasureInfo(string Name, string Abbreviation, string Category, decimal ConversionFactor, bool IsActive);

public record UnitOfMeasureGroupedDto(
    IEnumerable<UnitOfMeasureItemDto> MassUnits,
    IEnumerable<UnitOfMeasureItemDto> VolumeUnits,
    IEnumerable<UnitOfMeasureItemDto> DiscreteUnits
)
{
    public static UnitOfMeasureGroupedDto CreateFromEntities(IEnumerable<UnitOfMeasureInfo> units)
    {
        return new UnitOfMeasureGroupedDto(
            MassUnits: units.Where(u => u.Category == "masa").Select(u => new UnitOfMeasureItemDto(u.Abbreviation, u.Name)),
            VolumeUnits: units.Where(u => u.Category == "volumen").Select(u => new UnitOfMeasureItemDto(u.Abbreviation, u.Name)),
            DiscreteUnits: units.Where(u => u.Category == "unidades").Select(u => new UnitOfMeasureItemDto(u.Abbreviation, u.Name))
        );
    }
}
