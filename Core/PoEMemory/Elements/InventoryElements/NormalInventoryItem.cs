using System;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using GameOffsets;

namespace ExileCore.PoEMemory.Elements.InventoryElements
{
    public class NormalInventoryItem : Element
    {
        private Entity _item;
        private readonly Lazy<NormalInventoryItemOffsets> cachedValue;

        public NormalInventoryItem()
        {
            cachedValue = new Lazy<NormalInventoryItemOffsets>(() => M.Read<NormalInventoryItemOffsets>(Address));
        }

        public virtual int InventPosX => cachedValue.Value.InventPosX; //M.Read<int>(Address + InventPosXOff);
        public virtual int InventPosY => cachedValue.Value.InventPosY; //M.Read<int>(Address + InventPosYOff);
        public virtual int ItemWidth => cachedValue.Value.Width; //M.Read<int>(Address + WidthOff);
        public virtual int ItemHeight => cachedValue.Value.Height; //M.Read<int>(Address + HeightOff);

        public Entity Item
        {
            get
            {
                if (_item == null) _item = GetObject<Entity>(cachedValue.Value.Item);
                return _item;
            }
        }

        public ToolTipType toolTipType => ToolTipType.InventoryItem;

        //public Element ToolTip => ReadObject<Element>(Address + 0xB20);

        //0xB40 0xB48 some inf about image DDS
    }
}
