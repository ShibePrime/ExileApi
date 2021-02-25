using ExileCore.PoEMemory.Elements;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.MemoryObjects
{
    public class HarvestWindow : Element
    {

        public List<HarvestCraft> Crafts => GetChildFromIndices(8, 0, 1).GetChildrenAs<HarvestCraft>();

    }
}