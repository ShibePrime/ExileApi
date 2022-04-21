using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IngameDataOffsets
    {
        [FieldOffset(0xB0)] public byte CurrentAreaLevel;
        [FieldOffset(0x114)] public uint CurrentAreaHash;
        [FieldOffset(0x128)] public NativePtrArray MapStats;
        [FieldOffset(0x198)] public long LabDataPtr;
        [FieldOffset(0x680)] public long ServerData;
        [FieldOffset(0x678)] public long LocalPlayer; //3.17.4
        [FieldOffset(0x728)] public long EntityList; //3.17.4
        [FieldOffset(0x730)] public long EntitiesCount; //3.17.4
        [FieldOffset(0x8C0)] public TerrainData Terrain; //3.17.4
    }
}
