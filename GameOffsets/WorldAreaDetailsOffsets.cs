using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct WorldAreaDetailsOffsets
    {
        [FieldOffset(0x80)] public long AreaTemplate;
    }
}
