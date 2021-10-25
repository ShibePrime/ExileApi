using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using System;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.Elements
{
    public class InventoryElement : Element
    {
        private InventoryList _allInventories;
        private InventoryList AllInventories => _allInventories ??= GetObjectAt<InventoryList>(0x370);
        public Inventory this[InventoryIndex k] => AllInventories[k];

        public IList<Element> GetItemsInInventory()
        {
            var playerInventory = GetElementSlot(InventoryIndex.PlayerInventory);
            var items = playerInventory.Children;
            items.RemoveAt(0);
            return items;
        }

        public Element GetElementSlot(InventoryIndex inventoryIndex)
        {
            return inventoryIndex switch
            {
                InventoryIndex.None => throw new ArgumentOutOfRangeException(nameof(inventoryIndex)),
                InventoryIndex.Helm => EquippedItems.GetChildAtIndex(6),
                InventoryIndex.Amulet => EquippedItems.GetChildAtIndex(7),
                InventoryIndex.Chest => EquippedItems.GetChildAtIndex(8),
                InventoryIndex.LWeapon => EquippedItems.GetChildAtIndex(10),
                InventoryIndex.RWeapon => EquippedItems.GetChildAtIndex(9),
                InventoryIndex.LWeaponSwap => EquippedItems.GetChildAtIndex(12),
                InventoryIndex.RWeaponSwap => EquippedItems.GetChildAtIndex(11),
                InventoryIndex.LRing => EquippedItems.GetChildAtIndex(13),
                InventoryIndex.RRing => EquippedItems.GetChildAtIndex(14),
                InventoryIndex.Gloves => EquippedItems.GetChildAtIndex(15),
                InventoryIndex.Belt => EquippedItems.GetChildAtIndex(16),
                InventoryIndex.Boots => EquippedItems.GetChildAtIndex(17),
                InventoryIndex.PlayerInventory => EquippedItems.GetChildAtIndex(18),
                InventoryIndex.Flask => EquippedItems.GetChildAtIndex(19),
                InventoryIndex.Trinket => EquippedItems.GetChildAtIndex(20),
                InventoryIndex.BloodCrucible => EquippedItems.GetChildAtIndex(1),
                _ => throw new ArgumentOutOfRangeException(nameof(inventoryIndex))
            };
        }
        private Element EquippedItems => GetChildAtIndex(3);
    }
}
