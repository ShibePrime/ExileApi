using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ServerStashTabOffsets
    {
        [FieldOffset(0x08)] public NativeStringU Name;
        //[FieldOffset(0x20)] public int InventoryId; // TODO: Relocated from ServerStashTab.cs. Check if valid. (3.12.3)
        //[FieldOffset(0x26)] public ushort LinkedParentId; // TODO: Relacted from ServerStashTab.cs. Check if valid. (3.12.3)
        [FieldOffset(0x2c)] public uint Color;
		[FieldOffset(0x34)] public uint OfficerFlags;
		[FieldOffset(0x34)] public uint TabType;
		[FieldOffset(0x38)] public ushort DisplayIndex;
		[FieldOffset(0x3C)] public uint MemberFlags;
        [FieldOffset(0x3D)] public byte Flags;
    }
}
