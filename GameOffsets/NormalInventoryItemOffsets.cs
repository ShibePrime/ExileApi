using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct NormalInventoryItemOffsets
    {
        [FieldOffset(0x398)] public int InventPosX;
        [FieldOffset(0x39C)] public int InventPosY;
        [FieldOffset(0x3A0)] public int Width;
        [FieldOffset(0x3A4)] public int Height;
        [FieldOffset(0x390)] public long Item;
        [FieldOffset(0x14)] public int ToolTip;
    }
}
