using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IngameDataOffsets
    {
        [FieldOffset(0x80)] public long CurrentArea;
        [FieldOffset(0xA0)] public byte CurrentAreaLevel;
        [FieldOffset(0x104)] public uint CurrentAreaHash;
        [FieldOffset(0x118)] public NativePtrArray MapStats;
        [FieldOffset(0x124)] public long LabDataPtr;
        [FieldOffset(0x4F8)] public long LocalPlayer;
        [FieldOffset(0x5A0)] public long EntityList;
        [FieldOffset(0x5A8)] public long EntitiesCount;
        [FieldOffset(0x6A0+ 0x80)] public TerrainData Terrain;
    }
}
