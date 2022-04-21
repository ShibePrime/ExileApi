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
        [FieldOffset(0x238)] public Vector2 RelativeCoord;
        [FieldOffset(0x260)] public int GridX;//3.17.4
        [FieldOffset(0x264)] public int GridY;//3.17.4
        [FieldOffset(0x268)] public float Rotation; //3.17.4 =>  //in radians (0...2 PI)
        [FieldOffset(0x208)] public float Scale; //Incorrect as of 3.16.2b
        [FieldOffset(0x20C)] public int Size; //Incorrect as of 3.16.2b
        [FieldOffset(0x28C)] public Vector2 WorldPosition;//3.17.4
        [FieldOffset(0x28C)] public float WorldX;//3.17.4
        [FieldOffset(0x290)] public float WorldY;//3.17.4
    }
}
