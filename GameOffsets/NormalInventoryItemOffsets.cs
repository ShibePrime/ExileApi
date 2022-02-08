using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct NormalInventoryItemOffsets
    {
        [FieldOffset(0x3F8)] public long Tooltip;
        [FieldOffset(0x440)] public long Item;
        [FieldOffset(0x448)] public int InventPosX;
        [FieldOffset(0x44C)] public int InventPosY;
        [FieldOffset(0x450)] public int Width;
        [FieldOffset(0x454)] public int Height;
    }
}
