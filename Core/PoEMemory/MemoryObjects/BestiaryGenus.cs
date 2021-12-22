namespace ExileCore.PoEMemory.MemoryObjects
{
    public class BestiaryGenus : RemoteMemoryObject
    {
        private BestiaryGroup bestiaryGroup;
        private string genusId;
        private string icon;
        private string name;
        private string name2;
        public int Id { get; internal set; }
        public string GenusId => genusId ??= M.ReadStringU(M.Read<long>(Address));
        public string Name => name ??= M.ReadStringU(M.Read<long>(Address + 0x8));
        public BestiaryGroup BestiaryGroup => bestiaryGroup ??= TheGame.Files.BestiaryGroups.GetByAddress(M.Read<long>(Address + 0x10));
        public string Name2 => name2 ??= M.ReadStringU(M.Read<long>(Address + 0x20));
        public string Icon => icon ??= M.ReadStringU(M.Read<long>(Address + 0x28));
        public int MaxInStorage => M.Read<int>(Address + 0x30);

        public override string ToString()
        {
            return $"{Name}, MaxInStorage: {MaxInStorage}";
        }
    }
}
