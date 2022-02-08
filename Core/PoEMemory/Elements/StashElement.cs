using System;
using System.Collections.Generic;
using System.Linq;
using ExileCore.PoEMemory.MemoryObjects;

namespace ExileCore.PoEMemory.Elements
{
    public class StashElement : Element
    {
        public long TotalStashes => StashInventoryPanel?.ChildCount ?? 0;
        public Element ExitButton => Address != 0 ? GetObject<Element>(M.Read<long>(Address + 0x2D8)) : null;
        private Element StashTitlePanel => Address != 0 ? GetObject<Element>(M.Read<long>(Address + 0x2D0)) : null;
        private Element StashInventoryPanel => Address != 0 ? GetObject<Element>(M.Read<long>(Address + 0x2F8, 0x280, 0x980)) : null;
        public Element ViewAllStashButton => Address != 0 ? GetObject<Element>(M.Read<long>(Address + 0x2F8, 0x280, 0x988)) : null;
        public Element ViewAllStashPanel => Address != 0 ? GetObject<Element>(M.Read<long>(Address + 0x2F8, 0x280, 0x990)) : null;
        public Element ButtonStashTabListPin => Address != 0 ? GetObject<Element>(M.Read<long>(Address + 0x2F8, 0x280, 0x998)) : null;
        public int IndexVisibleStash => M.Read<int>(Address + 0x2F8, 0x280, 0x9E8);
        public Inventory VisibleStash => GetVisibleStash();
        public IList<string> AllStashNames => GetAllStashNames();
        public IList<Inventory> AllInventories => GetAllInventories();
        public IList<Element> TabListButtons => GetTabListButtons();

        private Inventory GetVisibleStash()
        {
            return GetStashInventoryByIndex(IndexVisibleStash);
        }

        private List<string> GetAllStashNames()
        {
            var ret = new List<string>();

            for (var i = 0; i < TotalStashes; i++)
            {
                ret.Add(GetStashName(i));
            }

            return ret;
        }

        private IList<Inventory> GetAllInventories()
        {
            var result = new List<Inventory>();

            for (var i = 0; i < TotalStashes; i++)
            {
                result.Add(GetStashInventoryByIndex(i));
            }

            return result;
        }

        public Inventory GetStashInventoryByIndex(int index) //This one is correct
        {
            if (index >= TotalStashes) return null;
            if (index < 0) return null;
            if (StashInventoryPanel.Children[index].ChildCount == 0) return null;

            Inventory stashInventoryByIndex = null;

            try
            {
                stashInventoryByIndex = StashInventoryPanel.Children[index].Children[0].Children[0].AsObject<Inventory>();
            }
            catch
            {
                DebugWindow.LogError($"Not found inventory stash for index: {index}");
            }

            return stashInventoryByIndex;
        }

        public IList<Element> GetTabListButtons()
        {
            var listChild = ViewAllStashPanel.Children.FirstOrDefault(x => x.ChildCount == TotalStashes);
            return listChild?.Children ?? new List<Element>();
        }

        public IList<Element> ViewAllStashPanelChildren
        {
            get
            {
                Element viewAllStashPanel = ViewAllStashPanel;
                if (viewAllStashPanel == null)
                {
                    return null;
                }
                return viewAllStashPanel.Children.Last(x => x.ChildCount == TotalStashes).Children.Where((Element x) =>
                {
                    IList<Element> children = x.Children;
                    return children != null && children.Count > 0;
                }).ToList();
            }
        }

        public string GetStashName(int index)
        {
            if (index >= TotalStashes || index < 0)
            {
                return string.Empty;
            }
            var viewAllStashPanelChildren = this.ViewAllStashPanelChildren;
            Element element;
            if (viewAllStashPanelChildren == null)
            {
                element = null;
            }
            else
            {
                var element2 = viewAllStashPanelChildren.ElementAt(index);
                IList<Element> children = element2.GetChildAtIndex(0).Children;
                if (element2 == null)
                {
                    element = null;
                }
                else
                {
                    element = children?.Last();
                }
            }
            return element == null ? string.Empty : element.Text;
        }
    }
}