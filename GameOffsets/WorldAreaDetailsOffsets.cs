using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct WorldAreaDetailsOffsets
    {
        [FieldOffset(0x88)] public long AreaTemplate;
    }
}
