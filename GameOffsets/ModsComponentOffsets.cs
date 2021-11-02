using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ModsComponentOffsets
    {
        public static readonly int ItemModRecordSize = 0x38;
        public static readonly int NameRecordSize = 0x10;
        public static readonly int NameOffset = 0x04;
        public static readonly int StatRecordSize = 0x20;

        [FieldOffset(0x30)] public NativePtrArray UniqueName;
        [FieldOffset(0xA8)] public bool Identified;
        [FieldOffset(0xAC)] public int ItemRarity;
        [FieldOffset(0xB8)] public NativePtrArray ImplicitModsArray;
        [FieldOffset(0xD0)] public NativePtrArray ExplicitModsArray;
        [FieldOffset(0xE8)] public NativePtrArray EnchantedModsArray;
        [FieldOffset(0x100)] public NativePtrArray ScourgeModsArray;
        [FieldOffset(0x1D8)] public NativePtrArray ImplicitStatsArray;
        [FieldOffset(0x218)] public NativePtrArray EnchantedStatsArray;
        [FieldOffset(0x258)] public NativePtrArray ScourgeStatsArray;
        [FieldOffset(0x298)] public NativePtrArray ExplicitStatsArray;
        [FieldOffset(0x2D8)] public NativePtrArray CraftedStatsArray;
        [FieldOffset(0x318)] public NativePtrArray FracturedStatsArray;
        [FieldOffset(0x7C8)] public int ItemLevel;
        [FieldOffset(0x7CC)] public int RequiredLevel;
        [FieldOffset(0x7D0)] public long IncubatorKey;
        [FieldOffset(0x7D8)] public short IncubatorKillCount;
        
        [FieldOffset(0x489)] public byte IsMirrored;
        [FieldOffset(0x48A)] public byte IsSplit;
        [FieldOffset(0x4AC)] public byte IsUsable;
    }
}
