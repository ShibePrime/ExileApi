namespace ExileCore.Shared.Enums
{
    /// <summary>
    /// Copied from GGPK -> InventoryType.dat file
    /// Possible improvement -> read it from the in memory dad file
    /// </summary>
    public enum InventoryTypeE
    {
        MainInventory = 0x00,
        BodyArmour,
        Weapon,
        Offhand,
        Helm,
        Amulet,
        Ring,
        Gloves,
        Boots,
        Belt,
        Flask,
        Cursor,
        Map,
        PassiveJewels,
        AnimatedArmour,
        Crafting,
        Leaguestone,
        Unused,
        Currency,
        Offer,
        Divination,
        Essence,
        Fragment,
        MapStashInv,
        UniqueStashInv,
        CraftingSpreeCurrency,
        CraftingSpreeItem,
        NormalOrQuad,
        AtlasWatchtower,
        Harvest1,
        Unknown30, // TODO: Check if these are 3.11 3-pack league tabs
        Unknown31,
        Harvest2,
        Unknown33,
        HeistAllyEquipment,
        Trinket,
        HeistLocker,
        StashRegularTab,
        Unknown38,
        Unknown39,
        Unknown40,
        Unknown41,
        Unknown42,
        Unknown43,
        UtzaalUltimatum
    }
}
