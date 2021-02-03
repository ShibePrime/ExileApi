using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IngameDataOffsets
    {
        [FieldOffset(0x78)] public long CurrentArea;
        [FieldOffset(0x90)] public byte CurrentAreaLevel;
        [FieldOffset(0xF4)] public uint CurrentAreaHash;
        [FieldOffset(0xF8)] public NativePtrArray MapStats;
        [FieldOffset(0x438)] public long LocalPlayer;
        [FieldOffset(0x11C)] public long LabDataPtr;
        [FieldOffset(0x4C0)] public long EntityList;
        [FieldOffset(0x4C8)] public long EntitiesCount;
        [FieldOffset(0x670)] public TerrainData Terrain;
    }
}
