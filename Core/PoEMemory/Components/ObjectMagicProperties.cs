using System;
using System.Collections.Generic;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using GameOffsets;
using SharpDX;

namespace ExileCore.PoEMemory.Components
{
    public class ObjectMagicProperties : Component
    {
        private readonly CachedValue<ObjectMagicPropertiesOffsets> _CachedValue;
        private long _ModsHash;
        private readonly List<string> _ModNamesList = new List<string>();
        private const int MOD_RECORDS_OFFSET = 0x18;
        private const int MOD_RECORD_SIZE = 0x38;
        private const int MOD_RECORD_KEY_OFFSET = 0x10;

        public ObjectMagicProperties()
        {
            _CachedValue =
                new FrameCache<ObjectMagicPropertiesOffsets>(() => M.Read<ObjectMagicPropertiesOffsets>(Address));
        }

        public ObjectMagicPropertiesOffsets ObjectMagicPropertiesOffsets => _CachedValue.Value;

        public MonsterRarity Rarity
        {
            get
            {
                if (Address != 0)
                {
                    return (MonsterRarity)ObjectMagicPropertiesOffsets.Rarity;
                }

                return MonsterRarity.Error;
            }
        }

        public long ModsHash => ObjectMagicPropertiesOffsets.Mods.GetHashCode();

        public List<string> Mods
        {
            get
            {
                if (Address == 0)
                {
                    return null;
                }

                if (_ModsHash == ModsHash)
                {
                    return _ModNamesList;
                }

                var first = ObjectMagicPropertiesOffsets.Mods.First;
                var last = ObjectMagicPropertiesOffsets.Mods.Last;
                var end = ObjectMagicPropertiesOffsets.Mods.First + 256 * MOD_RECORD_SIZE;

                if (first == 0 || last == 0 || last < first)
                {
                    return new List<string>();
                }

                last = Math.Min(last, end);
                for (var i = first + MOD_RECORDS_OFFSET; i < last; i += MOD_RECORD_SIZE)
                {
                    var read = M.Read<long>(i + MOD_RECORD_KEY_OFFSET, 0);
                    var mod = Cache.StringCache.Read($"{nameof(ObjectMagicProperties)}{read}",
                        () => M.ReadStringU(read));
                    _ModNamesList.Add(mod);
                }

                if (first == end)
                {
                    DebugWindow.LogMsg($"{nameof(ObjectMagicProperties)} read mods error address", 2, Color.OrangeRed);
                }

                _ModsHash = ModsHash;
                return _ModNamesList;
            }
        }
    }
}