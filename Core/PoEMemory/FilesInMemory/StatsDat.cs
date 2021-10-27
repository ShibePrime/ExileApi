using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Interfaces;

namespace ExileCore.PoEMemory.FilesInMemory
{
    public class StatsDat : FileInMemory
    {
        public StatsDat(IMemory m, Func<long> address) : base(m, address)
        {
            LoadItems();
        }

        public IDictionary<string, StatRecord> records { get; } = new Dictionary<string, StatRecord>(StringComparer.OrdinalIgnoreCase);
        public IDictionary<int, StatRecord> recordsById { get; } = new Dictionary<int, StatRecord>();

        public StatRecord GetStatByAddress(long address)
        {
            return records.Values.ToList().Find(x => x.Address == address);
        }

        private void LoadItems()
        {
            var iCounter = 1;

            foreach (var address in RecordAddresses())
            {
                var r = new StatRecord(M, address, iCounter++);
                records[r.Key] = r;
                recordsById[r.ID] = r;
            }
        }

        /// <summary>
        /// Code for GameStat.cs
        /// </summary>
        public string GameStatCode
        {
            get
            {
                var sb = new StringBuilder(
                    @"namespace ExileCore.Shared.Enums
{
    public enum GameStat
    {
");
                var enumValues = new HashSet<string>();
                foreach (var stat in recordsById.OrderBy(x => x.Key))
                {
                    var enumValue = GetNameFromId(stat.Value.Key);
                    if (!enumValues.Add(enumValue))
                        enumValue += "2";
                    sb.AppendFormat("\t\t/// <summary>\n\t\t/// {0}\n\t\t/// </summary>\n\t\t{1} = {2},\n\n",
                        stat.Value.Key, enumValue, stat.Key);
                }
                sb.Append(@"
    }
}
");
                return sb.ToString();
            }
        }

        private static string GetNameFromId(string id)
        {
            var sb = new StringBuilder();
            var uppercase = true;
            foreach (var ch in id)
            {
                switch (ch)
                {
                    case '_':
                        uppercase = true;
                        break;
                    case '+':
                        break;
                    case '-':
                        sb.Append('_');
                        break;
                    case '%':
                        sb.Append("Pct");
                        break;
                    default:
                        sb.Append(uppercase ? char.ToUpper(ch) : ch);
                        uppercase = false;
                        break;
                }
            }
            return sb.ToString();
        }

        public class StatRecord
        {
            private static string ReadCache(string address, Func<string> func)
            {
                return RemoteMemoryObject.Cache.StringCache.Read($"{nameof(ModsDat)}{address}", func);
            }

            public StatRecord(IMemory m, long address, int currentRecordId)
            {
                Address = address;

                Key = ReadCache($"{address}", () => m.ReadStringU(m.Read<long>(address), 512));
                Flag0 = m.Read<byte>(address + 0x8) != 0;
                IsLocal = m.Read<byte>(address + 0x9) != 0;
                IsWeaponLocal = m.Read<byte>(address + 0xA) != 0;
                Type = Key.Contains("%") ? StatType.Percents : (StatType)m.Read<int>(address + 0xB);
                Flag3 = m.Read<byte>(address + 0xF) != 0;
                UserFriendlyName = ReadCache($"{address + 0x10}", () => m.ReadStringU(m.Read<long>(address + 0x10), 512));
                ID = currentRecordId;
            }

            public string Key { get; }
            public long Address { get; }
            public StatType Type { get; }
            public bool Flag0 { get; }
            public bool IsLocal { get; }
            public bool IsWeaponLocal { get; }
            public bool Flag3 { get; }
            public string UserFriendlyName { get; }

            // more fields can be added (see in visualGGPK)
            public int ID { get; }

            public override string ToString()
            {
                return string.IsNullOrWhiteSpace(UserFriendlyName) ? Key : UserFriendlyName;
            }

            public string ValueToString(int val)
            {
                switch (Type)
                {
                    case StatType.Boolean:
                        return val != 0 ? "True" : "False";

                    case StatType.IntValue:
                    case StatType.Value2:
                        return val.ToString("+#;-#");
                    case StatType.Percents:
                    case StatType.Precents5:
                        return val.ToString("+#;-#") + "%";
                }

                return "";
            }
        }
    }
}
