using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ModsComponentOffsets
    {
        public static readonly int ItemModRecordSize = 0x28;
        public static readonly int NameRecordSize = 0x10;
        public static readonly int NameOffset = 0x04;
        public static readonly int StatRecordSize = 0x20;

        [FieldOffset(0x30)] public NativePtrArray UniqueName;
        [FieldOffset(0xA8)] public bool Identified;
        [FieldOffset(0xAC)] public int ItemRarity;
        [FieldOffset(0xB0)] public NativePtrArray implicitMods;
        [FieldOffset(0xC8)] public NativePtrArray explicitMods;
        [FieldOffset(0xE0)] public NativePtrArray enchantMods;
        [FieldOffset(0x1B0)] public NativePtrArray GetImplicitStats;
        [FieldOffset(0x1C8)] public NativePtrArray GetEnchantedStats;
        [FieldOffset(0x1E0)] public NativePtrArray GetStats;
        [FieldOffset(0x1F8)] public NativePtrArray GetCraftedStats;
        [FieldOffset(0x210)] public NativePtrArray GetFracturedStats;
        [FieldOffset(0x230)] public NativePtrArray GetSynthesizedStats;
        [FieldOffset(0x484)] public int ItemLevel;
        [FieldOffset(0x488)] public int RequiredLevel;
        [FieldOffset(0x47C)] public byte IsUsable;
        [FieldOffset(0x47D)] public byte IsMirrored;
    }
}