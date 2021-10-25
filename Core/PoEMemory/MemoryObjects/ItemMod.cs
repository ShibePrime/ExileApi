using System.Collections.Generic;

namespace ExileCore.PoEMemory.MemoryObjects
{
    public class ItemMod : RemoteMemoryObject
    {
        private string _DisplayName;
        private string _Group;
        private int _Level;
        private string _Name;
        private string _RawName;
        public int[] Values => GetValues();
        public int Value1 => Values[0];
        public int Value2 => Values[1];
        public int Value3 => Values[2];
        public int Value4 => Values[3];

        internal int[] GetValues()
        {
            return M.ReadAsArray<int>(M.Read<long>(Address), 4);
        }
        public string RawName
        {
            get
            {
                if (_RawName == null)
                    ParseName();

                return _RawName;
            }
        }

        public string Group
        {
            get
            {
                if (_RawName == null)
                    ParseName();

                return _Group;
            }
        }

        public string Name
        {
            get
            {
                if (_RawName == null)
                    ParseName();

                return _Name;
            }
        }

        public string DisplayName
        {
            get
            {
                if (_RawName == null)
                    ParseName();

                return _DisplayName;
            }
        }

        public int Level
        {
            get
            {
                if (_RawName == null)
                    ParseName();

                return _Level;
            }
        }

        private void ParseName()
        {
            var addr = M.Read<long>(Address + 0x28, 0);
            _RawName = Cache.StringCache.Read($"{nameof(ItemMod)}{addr}", () => M.ReadStringU(addr));

            _DisplayName = Cache.StringCache.Read($"{nameof(ItemMod)}{addr + 0x64}", () => M.ReadStringU(M.Read<long>(Address + 0x28, 0x64)));

            _Name = _RawName.Replace("_", ""); // Master Crafted mod can have underscore on the end, need to ignore
            _Group = Cache.StringCache.Read($"{nameof(ItemMod)}{addr + 0x70}", () => M.ReadStringU(M.Read<long>(Address + 0x28, 0x70)));
            var ixDigits = _Name.IndexOfAny("0123456789".ToCharArray());

            if (ixDigits < 0 || !int.TryParse(_Name.Substring(ixDigits), out _Level))
                _Level = 1;
            else
                _Name = _Name.Substring(0, ixDigits);
        }

        public override string ToString()
        {
            return $"{Name} ({Value1}, {Value2}, {Value3}, {Value4})";
        }
    }
}
