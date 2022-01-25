using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IngameDataOffsets
    {
        [FieldOffset(0x80)] public long CurrentArea;
        [FieldOffset(0xB0)] public byte CurrentAreaLevel;
        [FieldOffset(0x114)] public uint CurrentAreaHash;
        [FieldOffset(0x128)] public NativePtrArray MapStats;
        [FieldOffset(0x198)] public long LabDataPtr;
        [FieldOffset(0x580)] public long ServerData;
        [FieldOffset(0x588)] public long LocalPlayer;
        [FieldOffset(0x638)] public long EntityList;
        [FieldOffset(0x640)] public long EntitiesCount;
        [FieldOffset(0x7C8)] public TerrainData Terrain; //3.16.3
    }
}
