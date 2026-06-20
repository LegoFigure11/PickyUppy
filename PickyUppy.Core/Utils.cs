using PKHeX.Core;
using System.Reflection;

namespace PickyUppy.Core;

public static class Utils
{
    private static readonly Assembly thisAssembly;
    private static readonly Dictionary<string, string> resourceNameMap;

    public static readonly GameStrings Strings = GameInfo.GetStrings("en");

    static Utils()
    {
        thisAssembly = Assembly.GetExecutingAssembly();
        resourceNameMap = BuildLookup(thisAssembly.GetManifestResourceNames());
    }

    private static Dictionary<string, string> BuildLookup(IReadOnlyCollection<string> names)
    {
        var res = new Dictionary<string, string>(names.Count);
        foreach (var name in names)
        {
            var fname = GetFileName(name);
            res.TryAdd(fname, name);
        }
        return res;
    }

    private static string GetFileName(string name)
    {
        var period = name.LastIndexOf('.', name.Length - 6);
        var start = period + 1;
        System.Diagnostics.Debug.Assert(start != 0);

        // text file fetch excludes ".txt" (mixed case...); other extensions are used (all lowercase).
        return name.EndsWith(".txt", StringComparison.Ordinal)
            ? name[start..^4].ToLowerInvariant()
            : name[start..];
    }

    public static string? GetStringResource(string name)
    {
        if (!resourceNameMap.TryGetValue(name.ToLowerInvariant(), out var resourceName))
            return null;

        using var resource = thisAssembly.GetManifestResourceStream(resourceName);
        if (resource is null)
            return null;

        using var reader = new StreamReader(resource);
        return reader.ReadToEnd();
    }

    public static byte[]? GetBinaryResource(string name)
    {
        if (!resourceNameMap.TryGetValue(name.ToLowerInvariant(), out var resourceName))
            return null;

        using var resource = thisAssembly.GetManifestResourceStream(resourceName);
        if (resource is null)
            return null;

        using var reader = new BinaryReader(resource);
        return reader.ReadBytes((int)resource.Length);
    }

    public static Version? GetLatestVersion()
    {
        const string endpoint = "https://api.github.com/repos/LegoFigure11/PickyUppy/releases/latest";
        var response = NetUtil.GetStringFromURL(new Uri(endpoint));
        if (response is null) return null;

        const string tag = "tag_name";
        var index = response.IndexOf(tag, StringComparison.Ordinal);
        if (index == -1) return null;

        var first = response.IndexOf('"', index + tag.Length + 1) + 1;
        if (first == 0) return null;

        var second = response.IndexOf('"', first);
        if (second == -1) return null;

        var tagString = response.AsSpan()[first..second].TrimStart('v');

        var patchIndex = tagString.IndexOf('-');
        if (patchIndex != -1) tagString = tagString.ToString()[..patchIndex].AsSpan();

        return !Version.TryParse(tagString, out var latestVersion) ? null : latestVersion;
    }
}

public static class Strings
{
    public static string[] GetTables(string lang)
    {
        if (Tables.TryGetValue(lang, out string[]? value)) return value;
        return Tables["en"];
    }

    public static string[] GetSubTables(string lang)
    {
        if (SubTables.TryGetValue(lang, out string[]? value)) return value;
        return SubTables["en"];
    }

    private readonly static OrderedDictionary<string, string[]> Tables = new() {
        { "ja",      ["アイテム", "ボール", "カセキ", "きのみ"] },
        { "en",      ["Items", "Balls", "Fossils", "Berries"] },
        { "fr",      ["Objets", "Balls", "Fossiles", "Baies"] },
        { "it",      ["Strumenti", "Ball", "Fossili", "Bacche"] },
        { "de",      ["Items", "Bälle", "Fossilien", "Beeren"] },
        { "es",      ["Objetos", "Balls", "Fósiles", "Bayas"] },
        { "ko",      ["아이템", "볼", "화석", "나무열매를"] },
        { "zh-Hans", ["道具", "球", "化石", "树果"] },
        { "zh-Hant", ["道具", "球", "化石", "树果"] }
    };

    private readonly static OrderedDictionary<string, string[]> SubTables = new() {
        { "ja",      ["﻿(なし)", "げんきのアメ", "ちからのアメ", "まもりのアメ", "ちしきのアメ", "こころのアメ", "はやさのアメ", "Steps", "Daily/Near Mewtwo"] },
        { "en",      ["﻿(None)", "Health Candy", "Mighty Candy", "Tough Candy", "Smart Candy", "Courage Candy", "Quick Candy", "Steps", "Daily/Near Mewtwo"] },
        { "fr",      ["﻿(Aucun)", "Bonbon Santé", "Bonbon Force", "Bonbon Armure", "Bonbon Esprit", "Bonbon Mental", "Bonbon Sprint", "Steps", "Daily/Near Mewtwo"] },
        { "it",      ["﻿(None)", "Caramella vitalità", "Caramella potenza", "Caramella protezione", "Caramella acume", "Caramella intuito", "Caramella rapidità", "Steps", "Daily/Near Mewtwo"] },
        { "de",      ["(Keins)", "Energiebonbon", "Stärkebonbon", "Robustbonbon", "Gripsbonbon", "Mentalbonbon", "Flottbonbon", "Steps", "Daily/Near Mewtwo"] },
        { "es",      ["﻿Ningún", "Caramelo Vigor", "Caramelo Músculo", "Caramelo Aguante", "Caramelo Intelecto", "Caramelo Mente", "Caramelo Ímpetu", "Steps", "Daily/Near Mewtwo"] },
        { "ko",      ["﻿(없음)", "기력의사탕", "힘의사탕", "수비의사탕", "지식의사탕", "마음의사탕", "속도의사탕", "Steps", "Daily/Near Mewtwo"] },
        { "zh-Hans", ["(无)", "元气糖果", "力量糖果", "守护糖果", "知识糖果", "心灵糖果", "敏捷糖果", "Steps", "Daily/Near Mewtwo"] },
        { "zh-Hant", ["﻿(無)", "元氣糖果", "力量糖果", "守護糖果", "知識糖果", "心靈糖果", "敏捷糖果", "Steps", "Daily/Near Mewtwo"] }
    };
}
