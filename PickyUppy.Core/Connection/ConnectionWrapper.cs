using System.Net.Sockets;
using PickyUppy.Core.Structures;
using SysBot.Base;
using static SysBot.Base.SwitchCommand;

namespace PickyUppy.Core.Connection;

public class ConnectionWrapperAsync(SwitchConnectionConfig Config, Action<string> StatusUpdate) : Offsets
{
    public readonly ISwitchConnectionAsync Connection = Config.Protocol switch
    {
        SwitchProtocol.USB => new SwitchUSBAsync(Config.Port),
        _ => new SwitchSocketAsync(Config),
    };

    public bool Connected => IsConnected;
    private bool IsConnected { get; set; }
    private readonly bool CRLF = Config.Protocol is SwitchProtocol.WiFi;

    private string Title { get; set; } = string.Empty;


    public async Task<(bool, string)> Connect(CancellationToken token)
    {
        if (Connected) return (true, "");

        try
        {
            StatusUpdate("Connecting...");
            Connection.Connect();
            IsConnected = true;

            StatusUpdate("Detecting Game Version");
            Title = await Connection.GetTitleID(token).ConfigureAwait(false);
            if (Title != TitleIDP && Title != TitleIDE)
            {
                IsConnected = false;
                return (false, $"{Title} is not a Pokémon Let's GO! game.");
            }

            StatusUpdate("Configuring sysmodule...");
            var cmd = Configure(SwitchConfigureParameter.mainLoopSleepTime, 50, CRLF);
            await Connection.SendAsync(cmd, token).ConfigureAwait(false);

            StatusUpdate("Connected!");
            return (true, "");
        }
        catch (SocketException e)
        {
            IsConnected = false;
            return (false, e.Message);
        }
    }

    public async Task<(bool, string)> DisconnectAsync(CancellationToken token)
    {
        if (!Connected) return (true, "");

        try
        {
            StatusUpdate("Disconnecting controller");
            await DetachController(token).ConfigureAwait(false);

            StatusUpdate("Disconnecting...");
            Connection.Disconnect();
            IsConnected = false;
            StatusUpdate("Disconnected!");
            return (true, "");
        }
        catch (SocketException e)
        {
            IsConnected = false;
            return (false, e.Message);
        }
    }

    private ulong _currentSeedOffset = 0;
    public async Task<(ulong s0, ulong s1)> GetCurrentRNGState(CancellationToken token)
    {
        if (_currentSeedOffset == 0)
            _currentSeedOffset = await Connection.PointerAll(MainRNGPointer, token).ConfigureAwait(false);

        var data = await Connection.ReadBytesAbsoluteAsync(_currentSeedOffset, 16, token).ConfigureAwait(false);
        return (BitConverter.ToUInt64(data, 0), BitConverter.ToUInt64(data, 8));
    }

    public async Task SetCurrentRNGState(ulong _s0, ulong _s1, CancellationToken token)
    {
        if (_currentSeedOffset == 0)
            _currentSeedOffset = await Connection.PointerAll(MainRNGPointer, token).ConfigureAwait(false);

        var s0 = BitConverter.GetBytes(_s0);
        var s1 = BitConverter.GetBytes(_s1);
        await Connection.WriteBytesAbsoluteAsync(s0, _currentSeedOffset, token).ConfigureAwait(false);
        await Connection.WriteBytesAbsoluteAsync(s1, _currentSeedOffset + 8, token).ConfigureAwait(false);
    }

    public async Task DetachController(CancellationToken token)
    {
        await Connection.SendAsync(SwitchCommand.DetachController(CRLF), token).ConfigureAwait(false);
    }
}
