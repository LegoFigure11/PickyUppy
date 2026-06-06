namespace PickyUppy.Core.Structures;

public abstract class Offsets
{
    public const string LAGameVersion = "1.0.2";
    public const string TitleIDP = "010003F003A34000";
    public const string TitleIDE = "0100187003A36000";

    public IReadOnlyList<long> MainRNGPointer { get; } = [0x160D310, 0xA0, 0x0];
}
