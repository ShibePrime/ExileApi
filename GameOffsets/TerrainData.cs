using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct TerrainData
    {
        [FieldOffset(0x20)] public long NumCols;
        [FieldOffset(0x28)] public long NumRows;
        [FieldOffset(0xe0)] public NativePtrArray LayerMelee;
        [FieldOffset(0xf8)] public NativePtrArray LayerRanged;
        [FieldOffset(0x110)] public int BytesPerRow;
    }
}
