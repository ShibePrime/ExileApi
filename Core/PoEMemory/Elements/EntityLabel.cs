using ExileCore.PoEMemory.MemoryObjects;

namespace ExileCore.PoEMemory.Elements
{
    public class EntityLabel : Element
    {
        public int Length
        {
            get
            {
                var num = (int)M.Read<long>(Address + 0xC98);
                return num <= 0 || num > 1024 ? 0 : num;
            }
        }

        public int Capacity
        {
            get
            {
                var num = (int)M.Read<long>(Address + 0xCA0);
                return num <= 0 || num > 1024 ? 0 : num;
            }
        }

        public override string Text
        {
            get
            {
                var LabelLen = Length;

                if (LabelLen <= 0 || LabelLen > 1024)
                {
                    return string.Empty;

                    // return null;
                }

                if (Capacity >= 8)
                {
                    var read = M.Read<long>(Address + 0xC88);

                    return M.ReadStringU(read, LabelLen * 2, false);
                }

                return M.ReadStringU(Address + 0xC88, LabelLen * 2, false);
            }
        }

        public string Text2 => NativeStringReader.ReadString(Address + 0x3A0, M);

        public string Text3 => NativeStringReader.ReadStringLong(Address + 0x3A0, M);
    }
}
