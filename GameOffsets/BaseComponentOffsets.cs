using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct BaseComponentOffsets
    {
        // Pointer to whatever structure contains these data.
        [FieldOffset(0x10)] public long ItemCellsPtr;

        [FieldOffset(0xDC)] public byte InfluenceFlag;
        // This is incorrect, and I can't figure out the correct
        // offset at this time.  I suspect that the C++ code has
        // either (a) changed to use a value other than "1" for true,
        // or (b) this data is now stored elsewhere in the item struct.
        [FieldOffset(0xDF)] public bool isCorrupted;
        [FieldOffset(0xE2)] public bool isSynthesized;
        [FieldOffset(0x64)] public long PublicPricePtr;
    }
}
