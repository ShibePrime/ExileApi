using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct HeistContractComponentOffsets
    {
        [FieldOffset(0x8)] public long Owner;
        [FieldOffset(0x28)] public long Objective;
        [FieldOffset(0x30)] public NativePtrArray Requirements;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct HeistContractObjectiveOffsets
    {
        [FieldOffset(0x08)] public long TargetKey;
        [FieldOffset(0x14)] public long ClientKey;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct HeistContractRequirementOffsets
    {
        [FieldOffset(0x08)] public long JobKey;
        [FieldOffset(0x10)] public byte JobLevel;
    }
}