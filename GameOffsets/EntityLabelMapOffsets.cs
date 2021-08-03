using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct EntityLabelMapOffsets
    {
        public const int LabelOffset = 0x2E8;

        [FieldOffset(0x2F0)] public long EntityLabelMap;
    }
}
