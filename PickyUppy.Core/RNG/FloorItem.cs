using PickyUppy.Core.Enums;
using PickyUppy.Core.Interfaces;
using PKHeX.Core;

namespace PickyUppy.Core.RNG;

public static class FloorItem
{
    public static Task<List<ItemFrame>> Generate(ulong s0, ulong s1, ulong startAdv, ulong endAdv, ItemConfig cfg)
    {
        return Task.Run(() =>
        {
            List<ItemFrame> results = [];

            TableType table = cfg.Table;
            CandyType candy = cfg.Candy;

            string lang = cfg.Language;

            uint max = FloorItems.GetRandMax(table);

            var rng = new Xoroshiro128Plus(s0, s1);

            for (ulong i = startAdv; i <= endAdv; i++)
            {
                var (_s0, _s1) = rng.GetState();
                var inner = new Xoroshiro128Plus(_s0, _s1);
                rng.Next();
                var rand = (uint)inner.NextInt(max);

                var (item, qty) = FloorItems.GetItem(rand, table);

                if (cfg.FiltersEnabled && item != cfg.Target) continue;

                var idx = FloorItems.GetPKHeXItemIndex(item, candy);

                var f = new ItemFrame()
                {
                    _advances = i,
                    _seed0 = _s0,
                    _seed1 = _s1,
                    _lang = lang,

                    _itemIndex = idx,

                    Quantity = qty,
                };
                results.Add(f);
            }

            return results;
        });
    }
}
