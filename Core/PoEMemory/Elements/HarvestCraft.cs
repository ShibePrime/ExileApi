namespace ExileCore.PoEMemory.Elements
{
    public class HarvestCraft : Element
    {
        override public string Text => GetChildAtIndex(3).LongText;

        public string ParsedName => removeRandomFormatting(GetChildAtIndex(3).LongText);

        public string Count => GetChildAtIndex(1).GetChildAtIndex(0).Text;

        public string Level => GetChildAtIndex(4).Text;

        private string removeRandomFormatting(string text)
        {
            if (text != null)
            {
                if (text.IndexOf('<') != -1) //removing text coloring property (things like <white> and <craftcolor>)
                {
                    int start = text.IndexOf('<');
                    int end = text.IndexOf('>', start) + 1;
                    string result = text.Remove(start, end - start);
                    text = removeRandomFormatting(result);
                }
                text = text.Replace("{", "");
                text = text.Replace("}", "");
            }
            return text;
        }

    }
}
