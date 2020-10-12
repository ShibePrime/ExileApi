using System;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.FilesInMemory.Heist
{
    public class HeistNpc: RemoteMemoryObject
    {
        //private long NpcKey => M.Read<long>(Address + 0x08);
        //public string Id => M.ReadStringU(M.Read<long>(NpcKey + 0x00));
        //public string Name => M.ReadStringU(M.Read<long>(NpcKey + 0x08));
        //public string ShortName => M.ReadStringU(M.Read<long>(NpcKey + 0x2C));
        //public long MonsterVarietyKey => M.Read<long>(Address + 0x18);
        public long JobCount => Math.Max(0, Math.Min(10, M.Read<long>(Address + 0x20)));
        public List<HeistJob> Jobs => GetJobs(M.Read<long>(Address + 0x28));
        //public string CharacterPortrait => M.ReadStringU(M.Read<long>(Address + 0x30));
        public long StatCount => Math.Max(0, Math.Min(32, M.Read<long>(Address + 0x38)));
        public List<StatsDat.StatRecord> Stats => GetStats(M.Read<long>(Address + 0x40));
        public string DisplayName => M.ReadStringU(M.Read<long>(Address + 0x6C));

        //public string CharacterSilhouette => M.ReadStringU(Address + 0x78);
        //public string Icon => M.ReadStringU(Address + 0xB0);

        //public static long RecordSize => 0xE0;

        private List<StatsDat.StatRecord> GetStats(long source)
        {
            var stats = new List<StatsDat.StatRecord>();
            if ((source += 0x08) == 0) return stats;

            for (var i = 0; i < StatCount; ++i, source += 0x10) {
                stats.Add(TheGame.Files.Stats.GetStatByAddress(M.Read<long>(source, 0x08)));
            }

            return stats;
        }

        private List<HeistJob> GetJobs(long source)
        {
            var jobs = new List<HeistJob>();
            if ((source += 0x08) == 0) return jobs;

            for (var i = 0; i < JobCount; ++i, source += 0x10)
            {
                jobs.Add(TheGame.Files.HeistJobs.GetByAddress(M.Read<long>(source)));
            }

            return jobs;
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}