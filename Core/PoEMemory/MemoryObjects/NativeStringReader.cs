using ExileCore.Shared.Interfaces;

namespace ExileCore.PoEMemory.MemoryObjects
{
    public class NativeStringReader
    {
        public static string ReadString(long address, IMemory m, int length = 256)
        {
            var size = m.Read<uint>(address + 0x10);
            var capacity = m.Read<uint>(address + 0x18);

            return m.ReadStringU(8 <= capacity ? m.Read<long>(address) : address, length);
        }

        public static string ReadStringLong(long address, IMemory m, int length = 512)
        {
            var size = m.Read<int>(address + 0x10) * 2;
            var capacity = m.Read<uint>(address + 0x18);

            return m.ReadStringU(8 <= capacity ? m.Read<long>(address) : address, length);
        }
    }
}
