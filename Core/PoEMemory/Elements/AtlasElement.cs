using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore.PoEMemory.Elements
{
    public class AtlasElement : Element
    {
        public List<Element> InventorySlots => GetWatchtowers(); //TODO

        private List<Element> GetWatchtowers()
        {
            return Children[0].Children.Skip(1).Take(8).ToList();
        }
    }
}
