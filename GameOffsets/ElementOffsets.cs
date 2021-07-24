using System.Runtime.InteropServices;
using GameOffsets.Native;
using SharpDX;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ElementOffsets
    {
        public const int OffsetBuffers = 0x6EC;

        [FieldOffset(0x20)] public long SelfPointer; // Useful for valid check
        [FieldOffset(0x40)] public long ChildStart;
        [FieldOffset(0x40)] public NativePtrArray Childs;
        [FieldOffset(0x48)] public long ChildEnd;
        [FieldOffset(0x90)] public long Root;
        [FieldOffset(0x98)] public long Parent; // Works for Items only.
        [FieldOffset(0xA0)] public Vector2 Position;
        [FieldOffset(0xA0)] public float X;
        [FieldOffset(0xA4)] public float Y;
        [FieldOffset(0x110)] public float Scale;
        [FieldOffset(0x119)] public byte IsVisibleLocal;

        [FieldOffset(0x118)] public uint ElementBorderColor;
        [FieldOffset(0x11C)] public uint ElementBackgroundColor;
        [FieldOffset(0x120)] public uint ElementOverlayColor;

        [FieldOffset(0x138)] public float Width;
        [FieldOffset(0x13C)] public float Height;
        [FieldOffset(0x180)] public bool isHighlighted; // Checks B Channel of Border (#00000000 to #E7B478FF highlighted)

        [FieldOffset(0x180)] public uint TextBoxBorderColor;
        [FieldOffset(0x180)] public uint TextBoxBackgroundColor;
        [FieldOffset(0x188)] public uint TextBoxOverlayColor;

        [FieldOffset(0x340)] public long Tooltip;

        //[FieldOffset(0x3CB)] public byte isShadow; // 0
        //[FieldOffset(0x3C9)] public byte isShadow2; // 1

        //[FieldOffset(0x3B0)] public NativeStringU TestString;
    }
}
