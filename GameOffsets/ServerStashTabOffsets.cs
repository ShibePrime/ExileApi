using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ServerStashTabOffsets
    {
        [FieldOffset(0x08)] public NativeStringU Name;
        [FieldOffset(0x28)] public int InventoryId; //3.17.1
        [FieldOffset(0x26)] public ushort LinkedParentId;
        [FieldOffset(0x2C)] public uint Color;
        [FieldOffset(0x30)] public byte MemberFlags;
        [FieldOffset(0x31)] public byte OfficerFlags;
        [FieldOffset(0x34)] public uint TabType;
        [FieldOffset(0x38)] public ushort DisplayIndex;
        [FieldOffset(0x3D)] public byte Flags;
        [FieldOffset(0x3F)] public uint AffinityFlags;
    }
}