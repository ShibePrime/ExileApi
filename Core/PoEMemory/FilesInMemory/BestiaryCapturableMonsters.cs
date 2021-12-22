using System;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Interfaces;

namespace ExileCore.PoEMemory.FilesInMemory
{
    public class BestiaryCapturableMonsters : UniversalFileWrapper<BestiaryCapturableMonster>
    {
        private int _IdCounter;

        public BestiaryCapturableMonsters(IMemory m, Func<long> address) : base(m, address)
        {
        }

        protected override void EntryAdded(long address, BestiaryCapturableMonster entry)
        {
            entry.Id = _IdCounter++;
        }
    }
}
