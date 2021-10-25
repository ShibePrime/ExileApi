using System;

namespace ExileCore.Shared.Enums
{
    [Flags]
    public enum InventoryTabPermissions : byte
    {
        None = 0,
        View = 1,
        Add = 2,
        Remove = 4
    }

    public enum InventoryTabType : uint
    {
        Normal,
        Premium,
        Todo2,
        Currency,
        Todo4,
        Map,
        Divination,
        Quad,
        Essence,
        Fragment,
        Todo10,
        Todo11,
        Delve,
        Blight,
        Metamorph,
        Delirium
    }

    [Flags]
    public enum InventoryTabFlags : byte
    {
        RemoveOnly = 1,
        Unknown2 = 2,
        Premium = 4,
        Unknown3 = 8,
        Unknown1 = 0x10,
        Public = 0x20,
        MapSeries = 0x40,
        Hidden = 0x80
    }

    [Flags]
    public enum InventoryTabAffinityFlags : ushort
    {
        Currency = 0x0008,
        Unique = 0x0010,
        Map = 0x0020,
        Divination = 0x0040,
        Essence = 0x0100,
        Fragment = 0x0200,
        Delve = 0x1000,
        Blight = 0x2000,
        Meta = 0x3000,
        Delirium = 0x4000
    }
}