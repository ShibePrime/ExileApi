using System;
using System.Collections.Generic;
using ExileCore.Shared.Enums;

namespace ExileCore.PoEMemory.MemoryObjects
{
    public class InventoryList : RemoteMemoryObject
    {
        public static int InventoryCount => 37;

        public Inventory this[InventoryIndex inv]
        {
            get
            {
                var num = (int) inv;

                if (num < 0 || num >= InventoryCount)
                    return null;

                return ReadObjectAt<Inventory>(num * 8);
            }
        }

        public List<Inventory> DebugInventories => _debug();

        private List<Inventory> _debug()
        {
            var list = new List<Inventory>();

            for (var num = 0; num < InventoryCount; ++num)
            {
                list.Add(ReadObjectAt<Inventory>(num * 8));
            }

            return list;
        }
    }
}
