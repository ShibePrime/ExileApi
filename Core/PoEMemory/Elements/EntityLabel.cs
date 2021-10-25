using ExileCore.PoEMemory.MemoryObjects;

namespace ExileCore.PoEMemory.Elements
{
    public class EntityLabel : Element
    {
        public int Length
        {
            get
            {
                var num = (int)M.Read<long>(Address + 0x3B0);
                return num <= 0 || num > 1024 ? 0 : num;
            }
        }

        public int Capacity
        {
            get
            {
                var num = (int)M.Read<long>(Address + 0x3B8);
                return num <= 0 || num > 1024 ? 0 : num;
            }
        }

        public override string Text
        {
            get
            {
                var length = Length;

                if (length <= 0 || length > 1024)
                {
                    return string.Empty;
                }

                var address = Capacity < 8 ? Address + 0x3A0 : M.Read<long>(Address + 0x3A0);
                return M.ReadStringU(address, length * 2, false);
            }
        }

        public string Text2 => NativeStringReader.ReadString(Address + 0x4A0, M);

        public string Text3 => NativeStringReader.ReadStringLong(Address + 0x608, M);
    }
}