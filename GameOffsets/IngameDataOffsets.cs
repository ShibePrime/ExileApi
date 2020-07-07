using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IngameDataOffsets
    {
        [FieldOffset(0x60)] public long CurrentArea;
        [FieldOffset(0x78)] public byte CurrentAreaLevel;
        [FieldOffset(0xF0)] public NativePtrArray MapStats;
        [FieldOffset(0xDC)] public uint CurrentAreaHash;
        [FieldOffset(0x400)] public long LocalPlayer;
        [FieldOffset(0x11C)] public long LabDataPtr;
        [FieldOffset(0x488)] public long EntityList;
        [FieldOffset(0x490)] public long EntitiesCount;
        [FieldOffset(0x608)] public TerrainData Terrain;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct TerrainData
    {
        [FieldOffset(0x18)] public long NumCols;
        [FieldOffset(0x20)] public long NumRows;
        [FieldOffset(0xd8)] public NativePtrArray Layer1;
        [FieldOffset(0xf0)] public NativePtrArray Layer2;
        [FieldOffset(0x108)] public int BytesPerRow;
    }
}
