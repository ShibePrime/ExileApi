using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ComponentLookup
    {
        [FieldOffset(0x30)]
        public long ComponentArray;

        [FieldOffset(0x38)]
        public long Capacity;

        [FieldOffset(0x48)]
        public long Counter;
    }
}