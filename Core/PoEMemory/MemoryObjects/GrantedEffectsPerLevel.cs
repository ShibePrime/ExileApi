using System;
using System.Collections.Generic;
using ExileCore.PoEMemory.FilesInMemory;

namespace ExileCore.PoEMemory.MemoryObjects
{
    public class GrantedEffectsPerLevel : RemoteMemoryObject
    {
        public SkillGemWrapper SkillGemWrapper => ReadObject<SkillGemWrapper>(Address);
        public int Level => M.Read<int>(Address + 0x10);
        public int RequiredLevel => M.Read<int>(Address + 0x7C);
        public int ManaMultiplier => M.Read<int>(Address + 0x80);

        //public int RequirementsComparer => M.Read<int>(Address + 0x84);

        public int EffectivenessOfAddedDamage => M.Read<int>(Address + 0x90);
        public int Cooldown => M.Read<int>(Address + 0x98);
        public float VaalSoulGainPreventionTime => M.Read<int>(Address + 0xE1) / 100.0f;

        public IEnumerable<Tuple<StatsDat.StatRecord, int>> Costs
        {
            get
            {
                var result = new List<Tuple<StatsDat.StatRecord, int>>();

                var costsCount = M.Read<int>(Address + 0xF1);
                var pointerToTypes = M.Read<long>(Address + 0x109);
                var pointerToValues = M.Read<long>(Address + 0xF9);

                for (var i = 0; i < costsCount; i++)
                {
                    var statAddress = M.Read<long>(pointerToTypes, 0x08);
                    var stat = TheGame.Files.Stats.GetStatByAddress(statAddress);
                    var value = M.Read<int>(pointerToValues);

                    result.Add(new Tuple<StatsDat.StatRecord, int>(stat, value));

                    pointerToTypes += 16;
                    pointerToValues += 4;
                }

                return result;
            }
        }
        public IEnumerable<Tuple<StatsDat.StatRecord, int>> Stats
        {
            get
            {
                var result = new List<Tuple<StatsDat.StatRecord, int>>();

                var statsCount = M.Read<int>(Address + 0x14);
                var pointerToStats = M.Read<long>(Address + 0x1c);
                
                for (var i = 0; i < statsCount; i++)
                {
                    var datPtr = M.Read<long>(pointerToStats);
                    var stat = TheGame.Files.Stats.GetStatByAddress(datPtr);
                    result.Add(new Tuple<StatsDat.StatRecord, int>(stat, ReadStatValue(i)));
                    pointerToStats += 16; //16 because we are reading each second pointer
                }

                return result;
            }
        }

        internal int ReadStatValue(int index)
        {
            return M.Read<int>(Address + 0x58 + index * 4);
        }

        [Obsolete("No longer exists as of 3.14. See the Cost property.", false)]
        public int ManaCost => 0;

        [Obsolete("No longer exists as of 3.14.", false)]
        public IEnumerable<string> Tags => new List<string>();

        [Obsolete("No longer exists as of 3.14.", false)]
        public IEnumerable<Tuple<StatsDat.StatRecord, int>> QualityStats => new List<Tuple<StatsDat.StatRecord, int>>();

        [Obsolete("No longer exists as of 3.14.", false)]
        public IEnumerable<StatsDat.StatRecord> TypeStats => new List<StatsDat.StatRecord>();
    }
}
