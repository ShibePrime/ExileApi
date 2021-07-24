using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct BuffOffsets
    {
        [FieldOffset(0x8)] public long Name;
        [FieldOffset(0x18)] public byte IsInvisible;
        [FieldOffset(0x19)] public byte IsRemovable;
        [FieldOffset(0x3E)] public byte Charges;
        [FieldOffset(0x18)] public float MaxTime;
        [FieldOffset(0x1C)] public float Timer;
    }
}
