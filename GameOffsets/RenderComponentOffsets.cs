using System.Runtime.InteropServices;
using GameOffsets.Native;
using SharpDX;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct RenderComponentOffsets
    {
        [FieldOffset(0x88)] public Vector3 Pos;
        [FieldOffset(0x94)] public Vector3 Bounds;
        [FieldOffset(0xA8)] public NativeStringU Name;
        [FieldOffset(0xCC)] public Vector3 Rotation;
        [FieldOffset(0xF0)] public float Height;
	}
}
