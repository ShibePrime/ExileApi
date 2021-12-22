using System;
using System.Collections.Generic;
using ExileCore.PoEMemory.FilesInMemory;

namespace ExileCore.PoEMemory.MemoryObjects.Heist
{
    public class HeistNpcRecord : RemoteMemoryObject
    {
        //private long NpcKey => M.Read<long>(Address + 0x08);
        //private long MonsterVarietyKey => M.Read<long>(Address + 0x18); 
        private long _JobCount => Math.Max(0, Math.Min(10, M.Read<long>(Address + 0x20)));
        public List<HeistJobRecord> Jobs => GetJobs(M.Read<long>(Address + 0x28));
        public string PortraitFile => M.ReadStringU(M.Read<long>(Address + 0x30));
        private long _StatCount => Math.Max(0, Math.Min(32, M.Read<long>(Address + 0x38)));

        public List<StatsDat.StatRecord> Stats => GetStats(M.Read<long>(Address + 0x40));
        //private long _Count50 => M.Read<long>(Address + 0x48);
        //public long Key50 => M.Read<long>(Address + 0x50);
        //public float Value58 => M.Read<float>(Address + 0x58);
        //public int Value5C => M.Read<int>(Address + 0x5C);
        //public float Value60 => M.Read<int>(Address + 0x60);
        //public long Key64 => M.Read<long>(Address + 0x60);
        public string Name => M.ReadStringU(M.Read<long>(Address + 0x6C));
        //public int Value74 => M.Read<int>(Address + 0x74);
        //public string SilhouetteFile => M.ReadStringU(M.Read<long>(Address + 0x78));
        //public int Value80 => M.Read<int>(Address + 0x80);
        //public int Value84 => M.Read<int>(Address + 0x84);
        //public long HeistNPCsKey => M.Read<long>(Address + 0x88);
        //private long _Count98 => M.Read<long>(Address + 0x90);
        //public long Key98 => M.Read<long>(Address + 0x98);
        //public string ActiveNPCIcon => M.ReadStringU(M.Read<long>(Address + 0xB0));
        //public long HeistJobsKey => M.Read<long>(Address + 0xC0);
        //private long _RelatedAchievementsCount => M.Read<long>(Address + 0xC8);
        //public long RelatedAchievementsKey => M.Read<long>(Address + 0xD0);
        //public string AOFile => M.ReadStringU(M.Read<long>(Address + 0xD8));

        private List<StatsDat.StatRecord> GetStats(long source)
        {
            var stats = new List<StatsDat.StatRecord>();
            if (source == 0) return stats;

            for (var i = 0; i < _StatCount; ++i, source += 0x10)
            {
                stats.Add(TheGame.Files.Stats.GetStatByAddress(M.Read<long>(source, 0x0)));
            }

            return stats;
        }

        private List<HeistJobRecord> GetJobs(long source)
        {
            var jobs = new List<HeistJobRecord>();
            if (source == 0) return jobs;

            for (var i = 0; i < _JobCount; ++i, source += 0x10)
            {
                jobs.Add(TheGame.Files.HeistJobs.GetByAddress(M.Read<long>(source)));
            }

            return jobs;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}