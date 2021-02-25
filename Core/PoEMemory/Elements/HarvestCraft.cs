namespace ExileCore.PoEMemory.Elements
{
    public class HarvestCraft : Element
    {
        override public string Text => GetChildAtIndex(3).Text;

        public string Count => GetChildAtIndex(1).GetChildAtIndex(0).Text;

        public string Level => GetChildAtIndex(4).Text;
    }
}
