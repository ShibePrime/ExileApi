using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IngameDataOffsets
    {
        [FieldOffset(0x0A8)] public byte CurrentAreaLevel;
        [FieldOffset(0x10C)] public uint CurrentAreaHash;
        [FieldOffset(0x120)] public NativePtrArray MapStats;
        [FieldOffset(0x260)] public long LabDataPtr; //May be incorrect
        [FieldOffset(0x778)] public long ServerData;
        [FieldOffset(0x780)] public long LocalPlayer;
        [FieldOffset(0x830)] public long EntityList;
        [FieldOffset(0x838)] public long EntitiesCount;
        [FieldOffset(0x9C8)] public TerrainData Terrain;

    }
}
