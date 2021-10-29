using System;
using System.Collections.Generic;
using ExileCore.Shared;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Interfaces;
using GameOffsets;

namespace ExileCore.PoEMemory.FilesInMemory
{
    public class ModsDat : FileInMemory
    {
        public ModsDat(IMemory m, Func<long> address, StatsDat sDat, TagsDat tagsDat) : base(m, address)
        {
            LoadItems(sDat, tagsDat);
        }

        public IDictionary<string, ModRecord> records { get; } =
            new Dictionary<string, ModRecord>(StringComparer.OrdinalIgnoreCase);

        public IDictionary<long, ModRecord> DictionaryRecords { get; } = new Dictionary<long, ModRecord>();

        public IDictionary<Tuple<string, ModType>, List<ModRecord>> recordsByTier { get; } =
            new Dictionary<Tuple<string, ModType>, List<ModRecord>>();

        public ModRecord GetModByAddress(long address)
        {
            DictionaryRecords.TryGetValue(address, out var result);
            return result;
        }

        private void LoadItems(StatsDat sDat, TagsDat tagsDat)
        {
            foreach (var address in RecordAddresses())
            {
                var r = new ModRecord(M, sDat, tagsDat, address);

                if (records.ContainsKey(r.Key))
                {
                    continue;
                }

                DictionaryRecords.Add(address, r);
                records.Add(r.Key, r);

                if (r.Domain == ModDomain.Monster)
                {
                    continue;
                }

                var byTierKey = Tuple.Create(r.Group, r.AffixType);

                if (!recordsByTier.TryGetValue(byTierKey, out var groupMembers))
                {
                    groupMembers = new List<ModRecord>();
                    recordsByTier[byTierKey] = groupMembers;
                }

                groupMembers.Add(r);
            }

            foreach (var list in recordsByTier.Values)
            {
                list.Sort(ModRecord.ByLevelComparer);
            }
        }

        public class ModRecord
        {
            public const int NumberOfStats = 4;
            public static IComparer<ModRecord> ByLevelComparer = new LevelComparer();

            // more fields can be added (see in visualGGPK)

            private static string ReadCache(string address, Func<string> func)
            {
                return RemoteMemoryObject.Cache.StringCache.Read($"{nameof(ModsDat)}{address}", func);
            }

            public ModRecord(IMemory m, StatsDat sDat, TagsDat tagsDat, long address)
            {
                Address = address;
                var modRecord = m.Read<ModsRecordOffsets>(address);

                Key = ReadCache($"{modRecord.Key.buf}", () => modRecord.Key.ToString(m));

                Unknown8 = modRecord.Unknown8;
                MinLevel = modRecord.MinLevel;

                var read = m.Read<long>(modRecord.TypeName);
                TypeName = ReadCache($"{read}", () => m.ReadStringU(read, 255));

                var statAddresses = new long[NumberOfStats]
                {
                    modRecord.StatNames1,
                    modRecord.StatNames2,
                    modRecord.StatNames3,
                    modRecord.StatName4
                };

                StatNames = new StatsDat.StatRecord[NumberOfStats];

                for (var i = 0; i < NumberOfStats; ++i)
                {
                    if (statAddresses[i] == 0)
                    {
                        StatNames[i] = null;
                        continue;
                    }

                    var statId = m.Read<long>(statAddresses[i]);

                    if (statId == 0)
                    {
                        StatNames[i] = null;
                        continue;
                    }

                    var key = RemoteMemoryObject.Cache.StringCache.Read($"{nameof(StatsDat)}{statAddresses[i]}",
                        () => m.ReadStringU(statId, 512));

                    if (!sDat.records.TryGetValue(key, out StatNames[i]))
                    {
                        DebugWindow.LogError($"ModsDat => Stat '{key}' not found. (@{statId})");
                    }
                }

                Domain = (ModDomain)modRecord.Domain;
                UserFriendlyName = ReadCache($"{modRecord.UserFriendlyName}",
                    () => m.ReadStringU(modRecord.UserFriendlyName));
                AffixType = (ModType)modRecord.AffixType;
                Group = ReadCache($"{modRecord.Group}", () => m.ReadStringU(modRecord.Group));

                StatRange = new[]
                {
                    new IntRange(modRecord.StatRange1, modRecord.StatRange2),
                    new IntRange(modRecord.StatRange3, modRecord.StatRange4),
                    new IntRange(modRecord.StatRange5, modRecord.StatRange6),
                    new IntRange(modRecord.StatRange7, modRecord.StatRange8)
                };

                Tags = new TagsDat.TagRecord[modRecord.Tags];
                var ta = modRecord.ta;

                for (var i = 0; i < Tags.Length; i++)
                {
                    var ii = ta + 0x10 * i;
                    var l = m.Read<long>(ii, 0);

                    Tags[i] = tagsDat.Records[ReadCache($"{l}", () => m.ReadStringU(l, 255))];
                }

                TagChances = new Dictionary<string, int>(modRecord.TagChances);
                var tc = modRecord.tc;

                for (var i = 0; i < Tags.Length; i++)
                {
                    TagChances[Tags[i].Key] = m.Read<int>(tc + 4 * i);
                }

                IsEssence = modRecord.IsEssence == 0x01;
                Tier = ReadCache($"{modRecord.Tier}", () => m.ReadStringU(modRecord.Tier));
            }

            public long Address { get; }
            public string Key { get; }
            public ModType AffixType { get; }
            public ModDomain Domain { get; }
            public string Group { get; }
            public int MinLevel { get; }
            public StatsDat.StatRecord[] StatNames { get; }
            public IntRange[] StatRange { get; }
            public IDictionary<string, int> TagChances { get; }
            public TagsDat.TagRecord[] Tags { get; }
            public long Unknown8 { get; }
            public string UserFriendlyName { get; }
            public bool IsEssence { get; }
            public string Tier { get; }
            public string TypeName { get; }

            public override string ToString()
            {
                return $"Name: {UserFriendlyName}, Key: {Key}, MinLevel: {MinLevel}";
            }

            private class LevelComparer : IComparer<ModRecord>
            {
                public int Compare(ModRecord x, ModRecord y)
                {
                    return -x.MinLevel + y.MinLevel;
                }
            }
        }
    }
}