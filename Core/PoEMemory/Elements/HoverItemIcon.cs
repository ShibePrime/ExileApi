using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using GameOffsets;

namespace ExileCore.PoEMemory.Elements
{
    public class HoverItemIcon : Element
    {
        private static readonly int InventPosXOff = Extensions.GetOffset<NormalInventoryItemOffsets>(nameof(NormalInventoryItemOffsets.InventPosX));
        private static readonly int InventPosYOff = Extensions.GetOffset<NormalInventoryItemOffsets>(nameof(NormalInventoryItemOffsets.InventPosY));

        private static readonly int InventTooltipOffset =
            Extensions.GetOffset<NormalInventoryItemOffsets>(nameof(NormalInventoryItemOffsets.Tooltip));

        private static readonly int InventItemOffset =
            Extensions.GetOffset<NormalInventoryItemOffsets>(nameof(NormalInventoryItemOffsets.Item));

        private ToolTipType? _TooltipType;
        public Element InventoryItemTooltip => ReadObject<Element>(Address + InventTooltipOffset);
        public Element ItemInChatTooltip => ReadObject<Element>(Address + 0x1F0);
        public ItemOnGroundTooltip ToolTipOnGround => TheGame.IngameState.IngameUi.ItemOnGroundTooltip;
        public int InventPosX => M.Read<int>(Address + InventPosXOff);
        public int InventPosY => M.Read<int>(Address + InventPosYOff);

        public ToolTipType ToolTipType => _TooltipType ??= GetToolTipType();

        public new Element Tooltip => ToolTipType switch
        {
            ToolTipType.ItemOnGround => ToolTipOnGround.Tooltip,
            ToolTipType.InventoryItem => InventoryItemTooltip,
            ToolTipType.ItemInChat => ItemInChatTooltip.Children[0].Children[1],
            _ => null,
        };

        public Element ItemFrame => ToolTipType switch
        {
            ToolTipType.ItemOnGround => ToolTipOnGround.ItemFrame,
            ToolTipType.ItemInChat => ItemInChatTooltip.Children[0],
            _ => null,
        };

        public Entity Item =>
            ToolTipType switch
            {
                ToolTipType.ItemOnGround => TheGame.IngameState.IngameUi.ItemsOnGroundLabelElement.ItemOnHover
                    ?.GetComponent<WorldItem>()?.ItemEntity,
                ToolTipType.InventoryItem => ReadObject<Entity>(Address + InventItemOffset),
                _ => null,
            };

        private ToolTipType GetToolTipType()
        {
            if (ItemInChatTooltip?.IsValid ?? false) return ToolTipType.ItemInChat;
            if (InventoryItemTooltip?.IsValid ?? false) return ToolTipType.InventoryItem;
            if (ToolTipOnGround?.IsValid ?? false) return ToolTipType.ItemOnGround;
            return ToolTipType.None;
        }
    }
}
