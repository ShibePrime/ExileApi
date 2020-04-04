using System;
using System.Collections.Generic;
using System.Linq;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Interfaces;

namespace ExileCore.PoEMemory.FilesInMemory
{
    public class Quests : UniversalFileWrapper<Quest>
    {
        public Quests(IMemory game, Func<long> address) : base(game, address)
        {
        }

        public new IList<Quest> EntriesList
        {
            get
            {
                CheckCache();
                return CachedEntriesList.ToList();
            }
        }

        public new Quest GetByAddress(long address)
        {
            return base.GetByAddress(address);
        }
    }
}
