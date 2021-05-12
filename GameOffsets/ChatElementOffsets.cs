using System.Runtime.InteropServices;
using GameOffsets.Native;
using SharpDX;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ChatElementOffsets
    {
        [FieldOffset(0x112)] public byte IsVisibleLocal;
        [FieldOffset(0x17F)] public bool IsActive; // This is true when the chat is open
    }
}
