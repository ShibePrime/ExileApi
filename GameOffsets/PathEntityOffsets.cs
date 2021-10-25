using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct PathEntityOffsets
    {
        [FieldOffset(0x00)] public long Path;
        [FieldOffset(0x10)] public long Length;
    }
}