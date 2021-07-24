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
        [FieldOffset(0x500)] public long LocalPlayer;
        [FieldOffset(0x5B0)] public long EntityList;
        [FieldOffset(0x5B8)] public long EntitiesCount;
        [FieldOffset(0x750)] public TerrainData Terrain;
    }
}
