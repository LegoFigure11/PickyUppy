using PKHeX.Core;

namespace PickyUppy.Core.Interfaces;

internal interface IBasicFrame
{
    string Advances { get; }
}

public class ItemFrame : IBasicFrame
{
    internal ulong _advances { get; set; } = 0;
    internal ulong _seed0 { get; set; } = 0;
    internal ulong _seed1 { get; set; } = 0;
    internal ushort _itemIndex { get; set; } = 0;
    internal string _lang { get; set; } = "en";
    private GameStrings _strings => GameInfo.GetStrings(_lang);
    private string[] _items => _strings.GetItemStrings(EntityContext.Gen7b);

    public string Advances => $"{_advances:N0}";
    public string Item => _items[_itemIndex];
    public byte Quantity { get; set; } = 1;
    public string Seed0 => $"{_seed0:X16}";
    public string Seed1 => $"{_seed1:X16}";
}
