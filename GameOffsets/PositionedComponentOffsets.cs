using System.Runtime.InteropServices;
using SharpDX;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct PositionedComponentOffsets
    {
        [FieldOffset(0x8)] public long OwnerAddress;
		[FieldOffset(0x15C)] public byte Reaction;
		[FieldOffset(0x168)] public int Size;
		[FieldOffset(0x1B0)] public Vector2 PrevPosition;
		[FieldOffset(0x1C8)] public Vector2 RelativeCoord;
		[FieldOffset(0x1EC)] public Vector2 GridPosition;
        [FieldOffset(0x1EC)] public int GridX;
        [FieldOffset(0x1F0)] public int GridY;
        [FieldOffset(0x1F4)] public float Rotation;
        [FieldOffset(0x21C)] public Vector2 WorldPosition;
        [FieldOffset(0x21C)] public float WorldX;
        [FieldOffset(0x220)] public float WorldY;
    }
}
