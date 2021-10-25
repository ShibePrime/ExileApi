using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct NormalInventoryItemOffsets
    {
        [FieldOffset(0x430)] public int InventPosX;
        [FieldOffset(0x434)] public int InventPosY;
        [FieldOffset(0x438)] public int Width;
        [FieldOffset(0x43C)] public int Height;
        [FieldOffset(0x428)] public long Item;
        [FieldOffset(0x14)] public int ToolTip;
    }
}
