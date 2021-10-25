namespace ExileCore.PoEMemory.Elements
{
    public class HarvestCraft : Element
    {
        public override string Text => GetChildAtIndex(3).LongText;

        public string ParsedName => RemoveRandomFormatting(GetChildAtIndex(3).LongText);

        public string Count => GetChildAtIndex(1).GetChildAtIndex(0).Text;

        public string Level => GetChildAtIndex(4).Text;

        private string RemoveRandomFormatting(string text)
        {
            if (text != null)
            {
                if (text.IndexOf('<') != -1) //removing text coloring property (things like <white> and <craftcolor>)
                {
                    var start = text.IndexOf('<');
                    var end = text.IndexOf('>', start) + 1;
                    var result = text.Remove(start, end - start);
                    text = RemoveRandomFormatting(result);
                }
                text = text.Replace("{", "");
                text = text.Replace("}", "");
            }
            return text;
        }

    }
}