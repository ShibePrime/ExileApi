using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct BaseComponentOffsets
    {
        [FieldOffset(0x10)] public long ItemCellsPtr;
        [FieldOffset(0x60)] public long PublicPricePtr;
        [FieldOffset(0xC6)] public byte InfluenceFlag;
        [FieldOffset(0xC7)] public byte isCorrupted;
        [FieldOffset(0xC8)] public int AbsorbedCorruptionCount;
        [FieldOffset(0xCC)] public int ScourgeLevel;
    }
}
