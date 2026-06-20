using PickyUppy.Core.Enums;

namespace PickyUppy.Core;

public static class FloorItems
{
    public static uint GetRandMax(TableType table) => table switch {
        TableType.CaveBalls   => 301,
        TableType.CaveBerries => 060,
        TableType.CaveFossils => 100,
        _                     => 100, // Game Corner
    };

    public static (Items Item, byte Quantity) GetItem(uint rand, TableType table, CandyType sub = CandyType.FossilDefault)
    {
        var item = GetItemFromTable(rand, table, sub);
        var qty = GetItemQuantity(rand, table);
        return (item, qty);
    }

    private static byte GetItemQuantity(uint rand, TableType table) => (byte)((table == TableType.CaveBalls && rand >= 276) ? 10 : 1);

    private static Items GetItemFromTable(uint rand, TableType table, CandyType sub = CandyType.FossilDefault) => table switch {
        TableType.CaveBalls   => GetCaveBallItem(rand),
        TableType.CaveBerries => GetCaveBerriesItem(rand),
        TableType.CaveFossils => sub == CandyType.FossilDefault ? GetCaveFossilsItem(rand) : GetMewtwoFossilsItem(rand),
        _                     => GetGameCornerItem(rand),
    };
    private static Items GetCaveBallItem(uint rand) => rand switch
    {
           275 => Items.MasterBall,
        >= 150 => Items.UltraBall,
        >=  50 => Items.GreatBall,
        >=   0 => Items.PokeBall,
    };

    private static Items GetCaveBerriesItem(uint rand) => rand switch
    {
        >= 50 => Items.GoldenPinapBerry,
        >= 30 => Items.GoldenNanabBerry,
        >=  0 => Items.GoldenRazzBerry,
    };

    private static Items GetCaveFossilsItem(uint rand) => rand switch {
        >= 50 => Items.MaxRevive,
        >= 30 => Items.DomeFossil,
        >= 10 => Items.HelixFossil,
        >=  0 => Items.OldAmber, 
    };

    private static Items GetMewtwoFossilsItem(uint rand) => rand switch
    {
        >= 60 => Items.DomeFossil,
        >= 20 => Items.HelixFossil,
        >=  0 => Items.OldAmber,
    };

    private static Items GetGameCornerItem(uint rand) => rand switch
    {
        >= 50 => Items.Berry,
        >= 20 => Items.Candy,
        >= 10 => Items.CandyL,
        >=  5 => Items.CandyXL,
        >=  3 => Items.PPUp,
        >=  2 => Items.PPMax,
        >=  1 => Items.BottleCap,
        >=  0 => Items.GoldBottleCap
    };

    public static ushort GetPKHeXItemIndex(Items item, CandyType candyTable = CandyType.Health) => item switch
    {
        Items.PokeBall         => 0x0004,
        Items.GreatBall        => 0x0003,
        Items.UltraBall        => 0x0002,
        Items.MasterBall       => 0x0001,

        Items.OldAmber         => 0x0067,
        Items.HelixFossil      => 0x0065,
        Items.DomeFossil       => 0x0066,
        Items.MaxRevive        => 0x001d,

        Items.GoldBottleCap    => 0x031c,
        Items.BottleCap        => 0x031b,
        Items.PPMax            => 0x0035,
        Items.PPUp             => 0x0033,

        Items.CandyXL          => (ushort)(0x03cc + (byte)candyTable),
        Items.CandyL           => (ushort)(0x03c6 + (byte)candyTable),
        Items.Candy            => (ushort)(0x03c0 + (byte)candyTable),
        Items.Berry            => (ushort)(0x00a4 + (ushort)(((byte)candyTable / 2) * 2)),

        Items.GoldenRazzBerry  => 0x035e,
        Items.GoldenNanabBerry => 0x0360,
        Items.GoldenPinapBerry => 0x0362,

        _ => 0x0000,
    };
}
