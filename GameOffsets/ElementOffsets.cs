using System.Runtime.InteropServices;
using GameOffsets.Native;
using SharpDX;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ElementOffsets
    {
        public const int OffsetBuffers = 0x6EC;

        [FieldOffset(0x18)] public long SelfPointer; // Useful for valid check
        [FieldOffset(0x38)] public long ChildStart;
        [FieldOffset(0x38)] public NativePtrArray Childs;
        [FieldOffset(0x40)] public long ChildEnd;
        [FieldOffset(0x88)] public long Root;
        [FieldOffset(0x90)] public long Parent; // Works for Items only.
        [FieldOffset(0x98)] public Vector2 Position;
        [FieldOffset(0x98)] public float X;
        [FieldOffset(0x9C)] public float Y;
        [FieldOffset(0x108)] public float Scale;
        [FieldOffset(0x111)] public byte IsVisibleLocal;

        [FieldOffset(0x110)] public uint ElementBorderColor;
        [FieldOffset(0x114)] public uint ElementBackgroundColor;
        [FieldOffset(0x118)] public uint ElementOverlayColor;

        [FieldOffset(0x130)] public float Width;
        [FieldOffset(0x134)] public float Height;
        [FieldOffset(0x178)] public bool isHighlighted; // Checks B Channel of Border (#00000000 to #E7B478FF highlighted)

        [FieldOffset(0x178)] public uint TextBoxBorderColor;
        [FieldOffset(0x17C)] public uint TextBoxBackgroundColor;
        [FieldOffset(0x180)] public uint TextBoxOverlayColor;

        [FieldOffset(0x338)] public long Tooltip;

        //[FieldOffset(0x3CB)] public byte isShadow; // 0
        //[FieldOffset(0x3C9)] public byte isShadow2; // 1

        //[FieldOffset(0x3B0)] public NativeStringU TestString;
    }
}
