using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct HeistEquipmentComponentOffsets
    {
        [FieldOffset(0x08)] public long OwnerKey;
        [FieldOffset(0x10)] public long DataKey;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct HeistEquipmentComponentDataOffsets
    {
        [FieldOffset(0x18)] public long HeistEquipmentKey;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct HeistEquipmentOffsets
    {
        [FieldOffset(0x08)] public long BaseItemKey;
        [FieldOffset(0x18)] public long RequiredJobKey;
        [FieldOffset(0x20)] public int RequiredJobMinimumLevel;
    }
}