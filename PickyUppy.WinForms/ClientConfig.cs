using SysBot.Base;

namespace PickyUppy.WinForms;

public class ClientConfig
{
    // Connection
    public string IP { get; set; } = "192.168.0.0";
    public int UsbPort { get; set; } = 0;
    public SwitchProtocol Protocol { get; set; } = SwitchProtocol.WiFi;

    // Language
    public string Language { get; set; } = "en";
    public bool HasShownLanguageSelectPopop { get; set; } = false;
}
