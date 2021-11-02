using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using GameOffsets;

namespace ExileCore.PoEMemory.Elements
{
    public class HoverItemIcon : Element
    {
        private static readonly int InventPosXOff =
            Extensions.GetOffset<NormalInventoryItemOffsets>(nameof(NormalInventoryItemOffsets.InventPosX));

        private static readonly int InventPosYOff =
            Extensions.GetOffset<NormalInventoryItemOffsets>(nameof(NormalInventoryItemOffsets.InventPosY));

        private static readonly int InventItemTooltipOff =
            Extensions.GetOffset<NormalInventoryItemOffsets>(nameof(NormalInventoryItemOffsets.Tooltip));

        private static readonly int InventItemOff =
            Extensions.GetOffset<NormalInventoryItemOffsets>(nameof(NormalInventoryItemOffsets.Item));

        private static readonly int ItemsOnGroundLabelElementOffset =
            Extensions.GetOffset<IngameUElementsOffsets>(nameof(IngameUElementsOffsets.itemsOnGroundLabelRoot));


        private ToolTipType? _ToolTip;
        public Element InventoryItemTooltip => ReadObject<Element>(Address + InventItemTooltipOff);
        public Element ItemInChatTooltip => ReadObject<Element>(Address + 0x1F0);
        public ItemOnGroundTooltip ToolTipOnGround => TheGame.IngameState.IngameUi.ItemOnGroundTooltip;
        public int InventPosX => M.Read<int>(Address + InventPosXOff);
        public int InventPosY => M.Read<int>(Address + InventPosYOff);

        public ToolTipType ToolTipType
        {
            get { return _ToolTip ??= GetToolTipType(); }
        }

        public new Element Tooltip
        {
            get
            {
                return ToolTipType switch
                {
                    ToolTipType.ItemOnGround => ToolTipOnGround.Tooltip,
                    ToolTipType.InventoryItem => InventoryItemTooltip,
                    ToolTipType.ItemInChat => ItemInChatTooltip.Children[0].Children[1],
                    _ => null
                };
            }
        }

        public Element ItemFrame
        {
            get
            {
                return ToolTipType switch
                {
                    ToolTipType.ItemOnGround => ToolTipOnGround.ItemFrame,
                    ToolTipType.ItemInChat => ItemInChatTooltip.Children[0].Children[0],
                    _ => null
                };
            }
        }

        public Entity Item
        {
            get
            {
                switch (ToolTipType)
                {
                    case ToolTipType.ItemOnGround:
                        var le = TheGame.IngameState.IngameUi.ReadObjectAt<ItemsOnGroundLabelElement>(
                            ItemsOnGroundLabelElementOffset);
                        var e = le?.ItemOnHover;
                        return e?.GetComponent<WorldItem>()?.ItemEntity;
                    case ToolTipType.InventoryItem:
                        return ReadObject<Entity>(Address + InventItemOff);
                    case ToolTipType.ItemInChat:
                        return null;
                }

                return null;
            }
        }

        private ToolTipType GetToolTipType()
        {
            if (InventoryItemTooltip != null && InventoryItemTooltip.IsVisible)
            {
                return ToolTipType.InventoryItem;
            }

            if (ToolTipOnGround != null && ToolTipOnGround.Tooltip != null && ToolTipOnGround.TooltipUI != null &&
                ToolTipOnGround.TooltipUI.IsVisible)
            {
                return ToolTipType.ItemOnGround;
            }

            if (ItemInChatTooltip != null && ItemInChatTooltip.IsVisible && ItemInChatTooltip.ChildCount > 0 &&
                ItemInChatTooltip.Children[0].ChildCount > 1 && ItemInChatTooltip.Children[0].Children[0].IsVisible &&
                ItemInChatTooltip.Children[0].Children[1].IsVisible)
            {
                return ToolTipType.ItemInChat;
            }

            return ToolTipType.None;
        }
    }
}