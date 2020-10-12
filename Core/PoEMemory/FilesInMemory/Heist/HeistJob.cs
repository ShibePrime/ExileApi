using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.FilesInMemory.Heist
{
    public class HeistJob : RemoteMemoryObject
    {
        private List<StatsDat.StatRecord> _Stats;
        public string Id => M.ReadStringU(M.Read<long>(Address));
        public string Name => M.ReadStringU(M.Read<long>(Address + 0x08));
        public string RequiredIcon => M.ReadStringU(M.Read<long>(Address + 0x10));
        public string Art => M.ReadStringU(M.Read<long>(Address + 0x18));
        public string SkillMapArt => M.ReadStringU(M.Read<long>(Address + 0x28));
        public IEnumerable<StatsDat.StatRecord> Stats
        {
            get
            {
                if (_Stats != null) return _Stats;
                
                _Stats = new List<StatsDat.StatRecord>();

                var first = Address + 0x30;
                var last = Address + 0x80;
                var size = (last - first) / 2;
                var statRecords = M.ReadSecondPointerArray_Count(first, (int) Math.Min(size, 5));
                _Stats = statRecords.Select(x => TheGame.Files.Stats.GetStatByAddress(x)).ToList();
            
                return _Stats;
            }
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
