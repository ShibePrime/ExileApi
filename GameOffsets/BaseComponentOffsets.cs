using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct BaseComponentOffsets
    {
        // Pointer to whatever structure contains these data.
        [FieldOffset(0x10)] public long ItemCellsPtr;
        [FieldOffset(0x60)] public long PublicPricePtr;

        [FieldOffset(0xD6)] public byte InfluenceFlag;
        [FieldOffset(0xD7)] public byte isCorrupted;
    }
}
