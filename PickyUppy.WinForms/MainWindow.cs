using PickyUppy.Core;
using PickyUppy.Core.Connection;
using PickyUppy.Core.Enums;
using PickyUppy.Core.Interfaces;
using PickyUppy.WinForms.Subforms;
using PKHeX.Core;
using SysBot.Base;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using static PickyUppy.Core.Utils;
using PickyUppy.Core.RNG;

namespace PickyUppy.WinForms;

public partial class MainWindow : Form
{
    private static CancellationTokenSource Source = new();
    private static CancellationTokenSource ResetSource = new();

    private static readonly Lock _connectLock = new();

    public ClientConfig Config;
    private ConnectionWrapperAsync ConnectionWrapper = default!;
    private readonly SwitchConnectionConfig ConnectionConfig;

    private bool stop;
    private bool reset;
    private bool readPause;
    private uint total;

    internal List<object> Frames = [];

    private readonly Version CurrentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version!;
    public static readonly Font BoldFont = new("Microsoft Sans Serif", 8, FontStyle.Bold);

    private static GameStrings Strings = default!;

    public MainWindow()
    {
        Config = new ClientConfig();
        var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        if (File.Exists(configPath))
        {
            var text = File.ReadAllText(configPath);
            Config = JsonSerializer.Deserialize<ClientConfig>(text)!;
        }
        else
        {
            Config = new();
        }

        ConnectionConfig = new()
        {
            IP = Config.IP,
            Port = Config.Protocol is SwitchProtocol.WiFi ? 6000 : Config.UsbPort,
            Protocol = Config.Protocol,
        };

        var v = CurrentVersion;
#if DEBUG
        var build = "";

        var asm = System.Reflection.Assembly.GetEntryAssembly();
        var gitVersionInformationType = asm?.GetType("GitVersionInformation");
        var sha = gitVersionInformationType?.GetField("ShortSha");

        if (sha is not null) build += $"#{sha.GetValue(null)}";

        var date = File.GetLastWriteTime(AppContext.BaseDirectory);
        build += $" (dev-{date:yyyyMMdd})";

#else
        var build = "";
#endif

        Text = $"PickyUppy v{v.Major}.{v.Minor}.{v.Build}{build}";

        if (!Config.HasShownLanguageSelectPopop)
        {
            var ResetSettingsForm = new LanguageSettings(ref Config, this);
            ResetSettingsForm.ShowDialog();
            if (ResetSettingsForm.DialogResult == DialogResult.OK)
            {
                Config.HasShownLanguageSelectPopop = true;
            }
        }

        Strings = GameInfo.GetStrings(Config.Language);




        InitializeComponent();
    }

    private void MainWindow_Load(object sender, EventArgs e)
    {
        CenterToScreen();

        if (Config.Protocol is SwitchProtocol.WiFi)
        {
            TB_SwitchIP.Text = Config.IP;
        }
        else
        {
            L_SwitchIP.Text = "USB Port:";
            TB_SwitchIP.Text = $"{Config.UsbPort}";
        }

        SetControlText("0", TB_InitialSeed0, TB_InitialSeed1);
        SetControlText(string.Empty, TB_CurrentAdvances, TB_AdvancesIncrease, TB_CurrentSeed0, TB_CurrentSeed1);

        TB_Status.Text = "Not Connected.";

        var locations = Strings.GetLocationNames(4, GameVersion.HGSS);
        CB_Location.Items.Add(locations[102]);
        CB_Location.Items.Add(locations[199]);
        CB_Location.SelectedIndex = 0;

        CheckForUpdates();
    }

    private void Connect(CancellationToken token)
    {
        Task.Run(
            async () =>
            {
                SetControlEnabledState(false, B_Connect);
                try
                {
                    ConnectionConfig.IP = TB_SwitchIP.Text;
                    (bool success, string err) = await ConnectionWrapper
                        .Connect(token)
                        .ConfigureAwait(false);
                    if (!success)
                    {
                        SetControlEnabledState(true, B_Connect);
                        this.DisplayMessageBox(err);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    SetControlEnabledState(true, B_Connect);
                    this.DisplayMessageBox(ex.Message);
                    return;
                }

                UpdateStatus("Reading RNG State...");

                ulong _s0, _s1;
                try
                {
                    (_s0, _s1) = await ConnectionWrapper.GetCurrentRNGState(token).ConfigureAwait(false);
                    SetControlText($"{_s0:X16}", TB_InitialSeed0, TB_CurrentSeed0);
                    SetControlText($"{_s1:X16}", TB_InitialSeed1, TB_CurrentSeed1);
                    SetControlText("0", TB_CurrentAdvances, TB_AdvancesIncrease);
                }
                catch (Exception ex)
                {
                    this.DisplayMessageBox($"Error occurred while reading initial RNG state: {ex.Message}");
                    return;
                }

                SetControlEnabledState(true, B_Disconnect, B_CopyToInitial);

                UpdateStatus("Monitoring RNG State...");
                try
                {
                    total = 0;
                    stop = false;
                    while (!stop)
                    {
                        if (ConnectionWrapper.Connected && !readPause)
                        {
                            var (s0, s1) = await ConnectionWrapper.GetCurrentRNGState(token).ConfigureAwait(false);
                            var adv = Core.RNG.RNGUtil.GetAdvancesPassed(_s0, _s1, s0, s1);
                            if (reset || adv > 0)
                            {
                                if (reset || adv == 50_000)
                                {
                                    total = 0;
                                    reset = false;
                                    adv = 0;
                                }
                                else
                                {
                                    total += adv;
                                }

                                _s0 = s0;
                                _s1 = s1;

                                SetControlText($"{_s0:X16}", TB_CurrentSeed0);
                                SetControlText($"{_s1:X16}", TB_CurrentSeed1);
                                SetControlText($"{total:N0}", TB_CurrentAdvances);
                                SetControlText($"{adv:N0}", TB_AdvancesIncrease);
                            }
                        }
                    }
                }
                catch
                {
                    // Ignored
                }
            },
            token
        );
    }

    private void Disconnect(CancellationToken token)
    {
        Task.Run(
            async () =>
            {
                SetControlEnabledState(false, B_Disconnect);
                stop = true;
                try
                {
                    var (success, err) = await ConnectionWrapper.DisconnectAsync(token).ConfigureAwait(false);
                    if (!success) this.DisplayMessageBox(err);
                }
                catch (Exception ex)
                {
                    this.DisplayMessageBox(ex.Message);
                }
                await Source.CancelAsync().ConfigureAwait(false);
                Source = new();
                await ResetSource.CancelAsync().ConfigureAwait(false);
                ResetSource = new();
                SetControlEnabledState(true, B_Connect);
            },
            token
        );
    }

    private void UpdateStatus(string status)
    {
        SetControlText(status, TB_Status);
    }

    public void SetControlText(string text, params object[] obj)
    {
        foreach (object o in obj)
        {
            if (o is not Control c)
                continue;

            if (InvokeRequired)
                Invoke(() => c.Text = text);
            else
                c.Text = text;
        }
    }

    public void SetControlEnabledState(bool state, params object[] obj)
    {
        foreach (object o in obj)
        {
            if (o is Control c)
            {
                if (InvokeRequired)
                    Invoke(() => c.Enabled = state);
                else
                    c.Enabled = state;
            }

            if (o is ToolStripMenuItem d)
            {
                if (InvokeRequired)
                    Invoke(() => d.Enabled = state);
                else
                    d.Enabled = state;
            }
        }
    }

    public void SetControlVisibleState(bool state, params object[] obj)
    {
        foreach (object o in obj)
        {
            if (o is Control c)
            {
                if (InvokeRequired)
                    Invoke(() => c.Visible = state);
                else
                    c.Visible = state;
            }

            if (o is DataGridViewColumn d)
            {
                if (InvokeRequired)
                    Invoke(() => d.Visible = state);
                else
                    d.Visible = state;
            }
        }
    }

    public void SetBindingSourceDataSource(object source, params object[] obj)
    {
        foreach (object o in obj)
        {
            if (o is not BindingSource b)
                continue;

            if (InvokeRequired)
                Invoke(() => b.DataSource = source);
            else
                b.DataSource = source;
        }
    }

    public void SetDataGridViewDataSource(object source, params object[] obj)
    {
        foreach (object o in obj)
        {
            if (o is not DataGridView d)
                continue;

            if (InvokeRequired)
            {
                Invoke(() =>
                {
                    d.AutoGenerateColumns = true;
                    d.DataSource = source;

                    d.Columns["Seed"]?.DisplayIndex = d.Columns.Count - 1;
                    d.Columns["HP"]?.Width = 50;
                    d.Columns["Atk"]?.Width = 50;
                    d.Columns["Def"]?.Width = 50;
                    d.Columns["SpA"]?.Width = 50;
                    d.Columns["SpD"]?.Width = 50;
                    d.Columns["Spe"]?.Width = 50;


                });
            }
            else
            {
                d.AutoGenerateColumns = true;
                d.DataSource = source;

                d.Columns["Seed"]?.DisplayIndex = d.Columns.Count - 1;
                d.Columns["HP"]?.Width = 50;
                d.Columns["Atk"]?.Width = 50;
                d.Columns["Def"]?.Width = 50;
                d.Columns["SpA"]?.Width = 50;
                d.Columns["SpD"]?.Width = 50;
                d.Columns["Spe"]?.Width = 50;
            }
        }
    }

    public void SetNUDValue(decimal val, params NumericUpDown[] nuds)
    {
        foreach (var nud in nuds)
        {
            if (InvokeRequired) Invoke(() => nud.Value = val);
            else nud.Value = val;
        }
    }

    public void SetComboBoxOption(string opt, params ComboBox[] cbs)
    {
        foreach (var cb in cbs)
        {
            if (InvokeRequired) Invoke(() => cb.SelectedIndex = cb.Items.IndexOf(opt));
            else cb.SelectedIndex = cb.Items.IndexOf(opt);
        }
    }

    public void SetComboBoxSelectedIndex(int idx, params ComboBox[] cbs)
    {
        foreach (var cb in cbs)
        {
            if (InvokeRequired) Invoke(() => cb.SelectedIndex = idx);
            else cb.SelectedIndex = idx;
        }
    }

    private void B_Connect_Click(object sender, EventArgs e)
    {
        lock (_connectLock)
        {
            if (ConnectionWrapper is { Connected: true })
                return;

            ConnectionWrapper = new(ConnectionConfig, UpdateStatus);
            Connect(Source.Token);
        }
    }

    private void B_Disconnect_Click(object sender, EventArgs e)
    {
        lock (_connectLock)
        {
            if (ConnectionWrapper is not { Connected: true })
                return;

            Disconnect(Source.Token);
        }
    }

    private static Nature GetFilterNatureType(int selected) => selected switch
    {
        0 => Nature.Random,
        _ => (Nature)(selected - 1),
    };

    private void B_IV_Max_Click(object sender, EventArgs e)
    {
        var st = ((Button)sender).Name.Replace("B_", string.Empty).Replace("_Max", string.Empty);
        var underscore = st.IndexOf('_');
        var page = st[..underscore];
        var skill = st[(underscore + 1)..];
        List<string> stats = ModifierKeys == Keys.Shift ? ["HP", "Atk", "Def", "SpA", "SpD", "Spe"] : [skill];
        var val = ModifierKeys == Keys.Control ? 30 : 31;
        foreach (var stat in stats)
        {
            var min = (NumericUpDown)Controls.Find($"NUD_{page}_{stat}_Min", true).FirstOrDefault()!;
            var max = (NumericUpDown)Controls.Find($"NUD_{page}_{stat}_Max", true).FirstOrDefault()!;
            min.Value = val;
            max.Value = val;
        }
    }

    private void B_IV_Min_Click(object sender, EventArgs e)
    {
        var st = ((Button)sender).Name.Replace("B_", string.Empty).Replace("_Min", string.Empty);
        var underscore = st.IndexOf('_');
        var page = st[..underscore];
        var skill = st[(underscore + 1)..];
        List<string> stats = ModifierKeys == Keys.Shift ? ["HP", "Atk", "Def", "SpA", "SpD", "Spe"] : [skill];
        var val = ModifierKeys == Keys.Control ? 1 : 0;
        foreach (var stat in stats)
        {
            var min = (NumericUpDown)Controls.Find($"NUD_{page}_{stat}_Min", true).FirstOrDefault()!;
            var max = (NumericUpDown)Controls.Find($"NUD_{page}_{stat}_Max", true).FirstOrDefault()!;
            min.Value = val;
            max.Value = val;
        }
    }

    private void IV_Label_Click(object sender, EventArgs e)
    {
        var st = ((Label)sender).Name.Replace("L_", string.Empty);
        var underscore = st.IndexOf('_');
        var page = st[..underscore];
        var skill = st[(underscore + 1)..];
        List<string> stats = ModifierKeys == Keys.Shift ? ["HP", "Atk", "Def", "SpA", "SpD", "Spe"] : [skill];
        foreach (var stat in stats)
        {
            var min = (NumericUpDown)Controls.Find($"NUD_{page}_{stat}_Min", true).FirstOrDefault()!;
            var max = (NumericUpDown)Controls.Find($"NUD_{page}_{stat}_Max", true).FirstOrDefault()!;
            var lab = (Label)Controls.Find($"L_{page}_{stat}Spacer", true).FirstOrDefault()!;
            min.Value = 0;
            max.Value = 31;
            if (lab.Text == "||")
            {
                lab.Text = "~";
                lab.Location = lab.Location with { X = lab.Location.X - 1 };
            }
        }
    }

    private void IV_Spacer_Click(object sender, EventArgs e)
    {
        var l = (Label)sender;
        if (l.Text == "~")
        {
            l.Text = "||";
            l.Location = l.Location with { X = l.Location.X + 1 };
        }
        else
        {
            l.Text = "~";
            l.Location = l.Location with { X = l.Location.X - 1 };
        }
    }

    private void TB_SwitchIP_TextChanged(object sender, EventArgs e)
    {
        if (Config.Protocol is SwitchProtocol.WiFi)
        {
            Config.IP = TB_SwitchIP.Text;
            ConnectionConfig.IP = TB_SwitchIP.Text;
        }
        else
        {
            if (int.TryParse(TB_SwitchIP.Text, out var port) && port >= 0)
            {
                Config.UsbPort = port;
                ConnectionConfig.Port = port;
                return;
            }

            MessageBox.Show("Please enter a valid numerical USB port.");
        }
    }

    private readonly JsonSerializerOptions options = new() { WriteIndented = true };
    private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
    {
        var configpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        string output = JsonSerializer.Serialize(Config, options);
        using StreamWriter sw = new(configpath);
        sw.Write(output);

        if (ConnectionWrapper is { Connected: true })
        {
            try
            {
                _ = ConnectionWrapper.DisconnectAsync(Source.Token).ConfigureAwait(false);
            }
            catch
            {
                // ignored
            }
        }

        Source.Cancel();
        Source = new();
        ResetSource.Cancel();
        ResetSource = new();
    }

    private void AllowOnlyHex_KeyPress(object sender, KeyPressEventArgs e)
    {
        var c = e.KeyChar;
        if (c != (char)Keys.Back && !char.IsControl(c))
        {
            if (!c.IsHex())
            {
                System.Media.SystemSounds.Asterisk.Play();
                e.Handled = true;
            }
        }
    }

    public void AllowOnlyNumerical_KeyPress(object sender, KeyPressEventArgs e)
    {
        var c = e.KeyChar;
        if (c != (char)Keys.Back && !char.IsControl(c))
        {
            if (!c.IsDec())
            {
                System.Media.SystemSounds.Asterisk.Play();
                e.Handled = true;
            }
        }
    }

    public void AllowOnlyIP_KeyPress(object sender, KeyPressEventArgs e)
    {
        var c = e.KeyChar;
        if (c == (char)Keys.Return)
        {
            B_Connect_Click(sender, EventArgs.Empty);
        }
        else if (c != (char)Keys.Back && !char.IsControl(c))
        {
            if (!c.IsDec(true))
            {
                System.Media.SystemSounds.Asterisk.Play();
                e.Handled = true;
            }
        }
    }

    public void State_HandlePaste(object sender, KeyEventArgs e)
    {
        if (e is not { Modifiers: Keys.Control, KeyCode: Keys.V } && e is not { Modifiers: Keys.Shift, KeyCode: Keys.Insert }) return;
        var n = string.Empty;
        var newline = 0;
        var str = Clipboard.GetText();
        if (str.Contains("0x")) str = str.Replace("0x", string.Empty);
        foreach (char c in str)
        {
            if (c.IsHex()) n += c;
            if (c == (char)Keys.Enter) newline++;
        }

        var l = n.Length;
        if (l == 0)
        {
            Clipboard.Clear();
            return;
        }
        if (l == 32 && newline <= 1)
        {
            ((Control)sender).Parent!.Controls.Find("TB_InitialSeed0", true).FirstOrDefault()!.Text = n[..16];
            ((Control)sender).Parent!.Controls.Find("TB_InitialSeed1", true).FirstOrDefault()!.Text = n[16..32];
        }
        else if (l > 16)
        {
            ((TextBox)sender).Text = n[..16];
        }
        else
        {
            Clipboard.SetText(n);
        }
    }

    public void Dec_HandlePaste(object sender, KeyEventArgs e)
    {
        if (e is not { Modifiers: Keys.Control, KeyCode: Keys.V } && e is not { Modifiers: Keys.Shift, KeyCode: Keys.Insert }) return;
        var n = string.Empty;

        foreach (char c in Clipboard.GetText())
        {
            if (c.IsDec()) n += c;
        }

        var l = n.Length;
        var tb = (TextBox)sender;
        var max = tb.MaxLength;
        if (l == 0)
        {
            Clipboard.Clear();
        }
        else if (l > max)
        {
            tb.Text = n[..max];
        }
        else
        {
            Clipboard.SetText(n);
        }
    }

    private void IP_HandlePaste(object sender, KeyEventArgs e)
    {
        if (e is not { Modifiers: Keys.Control, KeyCode: Keys.V } && e is not { Modifiers: Keys.Shift, KeyCode: Keys.Insert }) return;
        var n = string.Empty;

        foreach (char c in Clipboard.GetText())
        {
            if (c.IsDec(true)) n += c;
        }

        var l = n.Length;
        var tb = (TextBox)sender;
        var max = tb.MaxLength;
        if (l == 0)
        {
            Clipboard.Clear();
        }
        else if (l > max)
        {
            tb.Text = n[..max];
        }
        else
        {
            Clipboard.SetText(n);
        }
    }

    private void ValidateInputs()
    {
        // Initial
        /*var initial = TB_Static_Initial;
        if (string.IsNullOrEmpty(initial.GetText())) SetControlText("0", initial);*/

        // Advances
        /*Control[] tbs = [TB_Spawner_Advances, TB_Static_Advances, TB_BabyMode];
        foreach (var advances in tbs)
        {
            var adv = advances.GetText();
            if (string.IsNullOrEmpty(adv) || adv is "0") SetControlText("1", advances);
        }*/

        // Seed
        if (string.IsNullOrEmpty(TB_InitialSeed0.GetText())) SetControlText("0", TB_InitialSeed0);

        if (TB_InitialSeed0.GetText() is "0")
        {
            SetControlText("1337", TB_InitialSeed0);
        }
        SetControlText(TB_InitialSeed0.GetText().PadLeft(16, '0'), TB_InitialSeed0);

        if (string.IsNullOrEmpty(TB_InitialSeed1.GetText())) SetControlText("0", TB_InitialSeed1);

        if (TB_InitialSeed1.GetText() is "0")
        {
            SetControlText("1390", TB_InitialSeed1);
        }
        SetControlText(TB_InitialSeed1.GetText().PadLeft(16, '0'), TB_InitialSeed1);
    }

    private void CheckForUpdates()
    {
        Task.Run(async () =>
        {
            Version? latestVersion;
            try { latestVersion = GetLatestVersion(); }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception while checking for latest version: {ex}");
                return;
            }

            if (latestVersion is null || latestVersion <= CurrentVersion)
                return;

            while (!IsHandleCreated) // Wait for form to be ready
                await Task.Delay(2_000).ConfigureAwait(false);
            await InvokeAsync(() => NotifyNewVersionAvailable(latestVersion));
        });
    }

    private void NotifyNewVersionAvailable(Version version)
    {
        Text += $" - Update v{version.Major}.{version.Minor}.{version.Build} available!";

#if !DEBUG
        using Subforms.UpdateNotifPopup nup = new(CurrentVersion, version);
        if (nup.ShowDialog() == DialogResult.OK)
        {
            Process.Start(new ProcessStartInfo("https://github.com/LegoFigure11/PickyUppy/releases/")
            {
                UseShellExecute = true
            });
        }
#endif
    }

    /*private void B_Static_Search_Click(object sender, EventArgs e)
    {
        ValidateInputs();
        SetControlEnabledState(false, B_Static_Search);
        Task.Run(async () =>
        {
            var s0 = ulong.Parse(TB_InitialSeed0.GetText(), NumberStyles.AllowHexSpecifier);
            var s1 = ulong.Parse(TB_InitialSeed1.GetText(), NumberStyles.AllowHexSpecifier);
            var start = ulong.Parse(TB_Static_Initial.GetText());
            var end = ulong.Parse(TB_Static_Advances.GetText());

            var cfg = new StaticConfig()
            {
                SID = ushort.Parse(TB_SID.GetText()),
                TID = ushort.Parse(TB_TID.GetText()),

                UseDelay = CB_Static_Delay.GetIsChecked(),
                Delay = NUD_Static_Delay.GetValue(),

                TargetNature = GetFilterNatureType(CB_Static_Nature.GetSelectedIndex()),

                TargetMinIVs = [NUD_Static_HP_Min.GetValue(), NUD_Static_Atk_Min.GetValue(), NUD_Static_Def_Min.GetValue(), NUD_Static_SpA_Min.GetValue(), NUD_Static_SpD_Min.GetValue(), NUD_Static_Spe_Min.GetValue()],
                TargetMaxIVs = [NUD_Static_HP_Max.GetValue(), NUD_Static_Atk_Max.GetValue(), NUD_Static_Def_Max.GetValue(), NUD_Static_SpA_Max.GetValue(), NUD_Static_SpD_Max.GetValue(), NUD_Static_Spe_Max.GetValue()],
                SearchTypes = [GetIVSearchType(L_Static_HPSpacer.GetText()), GetIVSearchType(L_Static_AtkSpacer.GetText()), GetIVSearchType(L_Static_DefSpacer.GetText()), GetIVSearchType(L_Static_SpASpacer.GetText()), GetIVSearchType(L_Static_SpDSpacer.GetText()), GetIVSearchType(L_Static_SpeSpacer.GetText())],

                _pk = GetMainEncounter(CB_Static_Species.GetSelectedIndex()),

                FiltersEnabled = CB_Static_FiltersEnabled.GetIsChecked(),
            };

            (s0, s1) = RNGUtil.XoroshiroJump(s0, s1, start);

            var staticFrames = await Static.Generate(s0, s1, start, end, cfg);

            hasShifted = false;
            SetBindingSourceDataSource(staticFrames, BS_StaticResults);
            SetDataGridViewDataSource(BS_StaticResults, DGV_Results);
            SetControlEnabledState(true, B_Static_Search);
            Frames = [.. staticFrames.Cast<object>()];
        });
    }*/

    bool hasShifted = false;

    private void DGV_Results_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        var index = e.RowIndex;
        if (Frames.Count <= index) return;
        var row = DGV_Results.Rows[index];
        var result = Frames[index];

        if (!hasShifted)
        {
            DGV_Results.Columns["Height"]?.DisplayIndex = DGV_Results.ColumnCount - 1;
            DGV_Results.Columns["Weight"]?.DisplayIndex = DGV_Results.ColumnCount - 1;
            DGV_Results.Columns["GeneratorSeed"]?.DisplayIndex = DGV_Results.ColumnCount - 1;
            DGV_Results.Columns["PokemonSeed"]?.DisplayIndex = DGV_Results.ColumnCount - 1;
            DGV_Results.Columns["Seed0"]?.DisplayIndex = DGV_Results.ColumnCount - 1;
            DGV_Results.Columns["Seed1"]?.DisplayIndex = DGV_Results.ColumnCount - 1;
            hasShifted = true;
        }
    }

    private void B_CopyToInitial_Click(object sender, EventArgs e)
    {
#if DEBUG
        if (((Button)sender).Name == "B_CopyToInitial" && (ModifierKeys & Keys.Shift) == Keys.Shift)
        {
            Task.Run(
                async () =>
                {
                    try
                    {
                        ulong s0 = ulong.Parse(TB_InitialSeed0.Text, NumberStyles.AllowHexSpecifier);
                        ulong s1 = ulong.Parse(TB_InitialSeed1.Text, NumberStyles.AllowHexSpecifier);
                        await ConnectionWrapper.SetCurrentRNGState(s0, s1, Source.Token).ConfigureAwait(false);
                        reset = true;
                    }
                    catch (Exception ex)
                    {
                        this.DisplayMessageBox($"Something went wrong when writing the RNG state: {ex.Message}");
                    }
                }
            );
        }
        else
        {
#endif
            if (TB_CurrentSeed0.Text != string.Empty && TB_CurrentSeed1.Text != string.Empty)
            {
                var s0 = TB_CurrentSeed0.Text;
                var s1 = TB_CurrentSeed1.Text;

                SetControlText(s0, TB_InitialSeed0);
                SetControlText(s1, TB_InitialSeed1);

                reset = true;
            }
#if DEBUG
        }
#endif
    }

    private void B_Search_Click(object sender, EventArgs e)
    {
        SetControlEnabledState(false, B_Search);
        Task.Run(async () =>
        {
            var s0 = ulong.Parse(TB_InitialSeed0.GetText(), NumberStyles.AllowHexSpecifier);
            var s1 = ulong.Parse(TB_InitialSeed1.GetText(), NumberStyles.AllowHexSpecifier);
            var start = ulong.Parse(TB_Initial.GetText());
            var end = ulong.Parse(TB_Advances.GetText());

            var loc = CB_Location.GetSelectedIndex();
            var tab = CB_ItemTable.GetSelectedIndex();
            var can = CB_CandyTable.GetSelectedIndex();
            var kvp = (KeyValuePair<string, Items>?)CB_TargetItem.GetSelectedItem();
            var val = Items.MasterBall;
            if (kvp != null)
            {
                val = kvp.Value.Value;
            }

            var cfg = new ItemConfig()
            {
                FiltersEnabled = CB_FiltersEnabled.GetIsChecked(),
                Table = loc == 0 ? TableType.GameCorner : (TableType)tab,
                Candy = (CandyType)can,

                Target = val,

                Quantity = NUD_Quantity.GetValue(),

                Language = Config.Language,
            };

            (s0, s1) = RNGUtil.XoroshiroJump(s0, s1, start);

            var itemFrames = await FloorItem.Generate(s0, s1, start, end, cfg);

            hasShifted = false;
            SetBindingSourceDataSource(itemFrames, BS_Results);
            SetDataGridViewDataSource(BS_Results, DGV_Results);
            SetControlEnabledState(true, B_Search);
            Frames = [.. itemFrames.Cast<object>()];
        });
    }

    private void CB_Location_SelectedIndexChanged(object sender, EventArgs e)
    {
        var items = Core.Strings.GetTables(Config.Language);
        var idx = CB_Location.SelectedIndex;

        CB_ItemTable.Items.Clear();

        if (idx == 0)
        {
            // Game Corner
            CB_ItemTable.Items.Add(items[0]);
        }
        else
        {
            // Cerulean Cave
            CB_ItemTable.Items.Add(items[1]);
            CB_ItemTable.Items.Add(items[2]);
            CB_ItemTable.Items.Add(items[3]);
        }
        CB_ItemTable.SelectedIndex = 0;
    }

    private void CB_ItemTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        var loc = CB_Location.SelectedIndex;
        var idx = CB_ItemTable.SelectedIndex;

        var items = Core.Strings.GetSubTables(Config.Language);
        var pkhex = Strings.GetItemStrings(EntityContext.Gen7b);

        CB_CandyTable.Items.Clear();

        if (loc == 0)
        {
            // Game Corner
            CB_CandyTable.Items.Add(items[1]);
            CB_CandyTable.Items.Add(items[2]);
            CB_CandyTable.Items.Add(items[3]);
            CB_CandyTable.Items.Add(items[4]);
            CB_CandyTable.Items.Add(items[5]);
            CB_CandyTable.Items.Add(items[6]);
        }
        else
        {
            // Cerulean Cave
            CB_CandyTable.Items.Add(items[0]);

            var type = (TableType)idx;
            var max = FloorItems.GetRandMax(type);
            HashSet<Items> tableItems = [];
            for (var i = 0u; i < max; i++)
            {
                tableItems.Add(FloorItems.GetItem(i, type).Item);
            }

            CB_TargetItem.Items.Clear();

            foreach (var item in tableItems)
            {
                var index = FloorItems.GetPKHeXItemIndex(item);
                CB_TargetItem.Items.Add(new KeyValuePair<string, Items>(pkhex[index], item));
            }
            CB_TargetItem.DisplayMember = "Key";
            CB_TargetItem.ValueMember = "Value";
            CB_TargetItem.SelectedIndex = 0;
        }

        CB_CandyTable.SelectedIndex = 0;
    }

    private void CB_CandyTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        var loc = CB_Location.SelectedIndex;
        var tab = CB_ItemTable.SelectedIndex;

        if (loc == 0 && tab == 0)
        {
            // Game Corner
            var candy = (CandyType)CB_CandyTable.SelectedIndex;
            var pkhex = Strings.GetItemStrings(EntityContext.Gen7b);

            var type = TableType.GameCorner;
            var max = FloorItems.GetRandMax(type);
            HashSet<Items> tableItems = [];
            for (var i = 0u; i < max; i++)
            {
                tableItems.Add(FloorItems.GetItem(i, type).Item);
            }

            CB_TargetItem.Items.Clear();

            foreach (var item in tableItems)
            {
                var index = FloorItems.GetPKHeXItemIndex(item, candy);
                CB_TargetItem.Items.Add(new KeyValuePair<string, Items>(pkhex[index], item));
            }
            CB_TargetItem.DisplayMember = "Key";
            CB_TargetItem.ValueMember = "Value";
            CB_TargetItem.SelectedIndex = 0;

        }
    }

    private void CB_TargetItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        var kvp = (KeyValuePair<string, Items>?)CB_TargetItem.SelectedItem;
        if (kvp is not null)
        {
            var enabled = kvp.Value.Value is Items.UltraBall;
            var max = enabled ? 10 : 1;

            NUD_Quantity.Enabled = enabled;
            NUD_Quantity.Maximum = max;
            NUD_Quantity.Value = max;
        }
    }
}

