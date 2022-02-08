using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct WorldDataOffsets
    {
        [FieldOffset(0xA0)] public long WorldAreaDetails;
        [FieldOffset(0xA8)] public CameraOffsets CameraStruct;
    }
}
