using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct EntityOffsets
    {
        [FieldOffset(0x8)] public long EntityDetailsPtr;
        [FieldOffset(0x10)] public NativePtrArray ComponentListPtr;

        // [FieldOffset(0x40)] public uint Id;
        //  [FieldOffset(0x58)] public uint InventoryId;
        public override string ToString()
        {
            return $"EntityDetailsPtr: {EntityDetailsPtr} ComponentListPtr: {ComponentListPtr}";
        }
    }
}