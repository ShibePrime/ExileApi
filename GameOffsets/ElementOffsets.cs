using System.Runtime.InteropServices;
using GameOffsets.Native;
using SharpDX;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ElementOffsets
    {
        public const int OffsetBuffers = 0x6EC;

        [FieldOffset(0x28)] public long SelfPointer; // Useful for valid check
        [FieldOffset(0x30)] public long ChildStart;
        [FieldOffset(0x30)] public NativePtrArray Childs;
        [FieldOffset(0x38)] public long ChildEnd;
        [FieldOffset(0xD8)] public long Root;
        [FieldOffset(0xE0)] public long Parent; // Works for Items only.
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
        [FieldOffset(0x190)] public bool isHighlighted; // Checks B Channel of Border (#00000000 to #E7B478FF highlighted)

        [FieldOffset(0x190)] public uint TextBoxBorderColor;
        [FieldOffset(0x190)] public uint TextBoxBackgroundColor;
        [FieldOffset(0x194)] public uint TextBoxOverlayColor;

        [FieldOffset(0x340)] public long Tooltip;

        //[FieldOffset(0x3CB)] public byte isShadow; // 0
        //[FieldOffset(0x3C9)] public byte isShadow2; // 1

        //[FieldOffset(0x3B0)] public NativeStringU TestString;
    }
}