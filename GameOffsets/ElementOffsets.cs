using System.Runtime.InteropServices;
using GameOffsets.Native;
using SharpDX;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ElementOffsets
    {
        public const int OffsetBuffers = 0x6EC;

        [FieldOffset(0x28)] public long SelfPointer;
        [FieldOffset(0x30)] public long ChildStart;
        [FieldOffset(0x30)] public NativePtrArray Childs;
        [FieldOffset(0x38)] public long ChildEnd;
        [FieldOffset(0xD8)] public long Root;
        [FieldOffset(0xE0)] public long Parent;
        [FieldOffset(0xE8)] public Vector2 Position;
        [FieldOffset(0xE8)] public float X;
        [FieldOffset(0xEC)] public float Y;
        [FieldOffset(0x158)] public float Scale;
        [FieldOffset(0x161)] public byte IsVisibleLocal;

        [FieldOffset(0x160)] public uint ElementBorderColor;
        [FieldOffset(0x164)] public uint ElementBackgroundColor;
        [FieldOffset(0x168)] public uint ElementOverlayColor;

        [FieldOffset(0x180)] public float Width;
        [FieldOffset(0x184)] public float Height;

        [FieldOffset(0x190)] public uint TextBoxBorderColor;
        [FieldOffset(0x190)] public uint TextBoxBackgroundColor;
        [FieldOffset(0x194)] public uint TextBoxOverlayColor;

        [FieldOffset(0x1C0)] public uint HighlightBorderColor;
        [FieldOffset(0x1C3)] public bool isHighlighted;

        [FieldOffset(0x3E8)] public long Tooltip;
    }
}