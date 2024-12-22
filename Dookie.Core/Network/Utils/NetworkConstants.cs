namespace Dookie.Core.Network;

public static class NetworkConstants
{
    public const int ServerPort = 3074;
    public const string ServerKey = "XD";
    
    public const int ProtocolId = 1;
    public static readonly int PacketTypesCount = Enum.GetValues<PacketType>().Length;

    public const int MaxGameSequence = 1024;
    public const int HalfMaxGameSequence = MaxGameSequence / 2;
}