using System.Runtime.InteropServices;
using SharpDX;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct PositionedComponentOffsets
    {
        [FieldOffset(0x8)] public long OwnerAddress;
		[FieldOffset(0x159)] public byte Reaction;
		[FieldOffset(0x168)] public int Size;
		[FieldOffset(0x1B0)] public Vector2 PrevPosition;
		[FieldOffset(0x1C8)] public Vector2 RelativeCoord;
        [FieldOffset(0x1E8)] public int GridX;
        [FieldOffset(0x1EC)] public int GridY;
        [FieldOffset(0x1F0)] public float Rotation;
        [FieldOffset(0x214)] public Vector2 WorldPosition;
        [FieldOffset(0x214)] public float WorldX;
        [FieldOffset(0x218)] public float WorldY;
    }
}
