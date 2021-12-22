using System.Runtime.InteropServices;
using SharpDX;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct PositionedComponentOffsets
    {
        [FieldOffset(0x8)] public long OwnerAddress;
        [FieldOffset(0x1D9)] public byte Reaction;
        [FieldOffset(0x22C)] public Vector2 PrevPosition;
        [FieldOffset(0x244)] public Vector2 RelativeCoord;
        [FieldOffset(0x268)] public int GridX;
        [FieldOffset(0x26C)] public int GridY;
        [FieldOffset(0x1F0)] public float Rotation; //Incorrect as of 3.16.2b
        [FieldOffset(0x208)] public float Scale; //Incorrect as of 3.16.2b
        [FieldOffset(0x20C)] public int Size; //Incorrect as of 3.16.2b
        [FieldOffset(0x294)] public Vector2 WorldPosition;
        [FieldOffset(0x294)] public float WorldX;
        [FieldOffset(0x298)] public float WorldY;
    }
}