using PickyUppy.Core.Enums;

namespace PickyUppy.Core.Interfaces;

internal interface IGeneratorConfig
{
    public bool FiltersEnabled { get; set; }
}

public class ItemConfig : IGeneratorConfig
{
    public bool FiltersEnabled { get; set; } = true;
    public string Language { get; set; } = "en";
    public TableType Table { get; set; } = TableType.CaveBalls;
    public CandyType Candy { get; set; } = CandyType.Health;
    public bool Jump { get; set; } = false;

    public uint Quantity { get; set; } = 1;
    public Items Target { get; set; } = Items.MasterBall;
}
