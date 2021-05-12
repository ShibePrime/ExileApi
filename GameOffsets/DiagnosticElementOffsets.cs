using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct DiagnosticElementOffsets
    {
        [FieldOffset(0x0)] public long DiagnosticArray;
        [FieldOffset(0x08)] public int X;
        [FieldOffset(0x0C)] public int Y;
        [FieldOffset(0x10)] public int Width;
        [FieldOffset(0x14)] public int Height;
    }
}
