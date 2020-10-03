using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ServerInventoryOffsets
    {
        [FieldOffset(0x00)] public byte InventType;
        [FieldOffset(0x04)] public byte InventSlot;
        [FieldOffset(0x08)] public byte IsRequested; // TODO: Find out what this is & verify (3.12.2)
        [FieldOffset(0x0C)] public int Columns;
        [FieldOffset(0x10)] public int Rows;
        [FieldOffset(0x30)] public long InventoryItemsPtr;
        [FieldOffset(0x48)] public long InventorySlotItemsPtr;
        [FieldOffset(0x50)] public long CountItems;
        [FieldOffset(0x50)] public int TotalItemsCount;
        [FieldOffset(0x58)] public long Hash;
        [FieldOffset(0xA8)] public int ServerRequestCounter;
    }
}
