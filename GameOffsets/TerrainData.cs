using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct TerrainData
    {
        [FieldOffset(0x0)] public long NumCols;
        [FieldOffset(0x8)] public long NumRows;
        [FieldOffset(0xc0)] public NativePtrArray LayerMelee;
        [FieldOffset(0xd8)] public NativePtrArray LayerRanged;
        [FieldOffset(0xF0)] public int BytesPerRow;
    }
}
