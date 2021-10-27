using System.Runtime.InteropServices;
using GameOffsets.Native;
using SharpDX;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ChatElementOffsets
    {
        [FieldOffset(0x1C7)] public bool IsActive; // This is true when the chat is open
    }
}
