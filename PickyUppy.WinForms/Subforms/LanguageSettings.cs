using PKHeX.Core;
using System.Text.Json;

namespace PickyUppy.WinForms.Subforms;

public partial class LanguageSettings : Form
{
    private readonly ClientConfig _config;
    private readonly MainWindow _mainWindow;

    private static readonly byte[] langIDs = Language.GetAvailableGameLanguages(EntityContext.Gen7b).ToArray();

    public LanguageSettings(ref ClientConfig cfg, MainWindow m)
    {
        _config = cfg;
        _mainWindow = m;

        InitializeComponent();
    }

    private void LanguageSettings_FormClosing(object sender, FormClosingEventArgs e)
    {
        string output = JsonSerializer.Serialize(_config);
        using StreamWriter sw = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json"));
        sw.Write(output);

        _mainWindow.Config = _config;
    }

    private void LanugageSettings_Load(object sender, EventArgs e)
    {
        CenterToScreen();
        Activate();

        string[] languages = [
            "日本語",
            "English",
            "Français",
            "Italiano",
            "Deutsch",
            "Español",
            "한국어",
            "简体中文",
            "繁體中文",
        ];

        CB_Language.Items.AddRange(languages);
        CB_Language.SelectedIndex = 1;
    }

    private void CB_Language_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selected = (byte)(CB_Language.SelectedIndex + 1);
        if (selected >= 6) selected += 1;

        if (!langIDs.Contains(selected)) selected = 2;

        var code = Language.GetLanguageCode((LanguageID)selected);

        _config.Language = code;
    }
}
