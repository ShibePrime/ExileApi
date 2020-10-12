namespace ExileCore.PoEMemory.FilesInMemory.Heist
{
    public class HeistChestRewardType : RemoteMemoryObject
    {
        public string Id => M.ReadStringU(M.Read<long>(Address));
        public string Art => M.ReadStringU(M.Read<long>(Address + 0x08));
        public string Name => M.ReadStringU(M.Read<long>(Address + 0x10));
        public int MinimumDropLevel => M.Read<int>(Address + 0x28);
        public int MaximumDropLevel => M.Read<int>(Address + 0x2C);
        public int Unknown30 => M.Read<int>(Address + 0x30);
        public string RoomName => M.ReadStringU(M.Read<long>(Address + 0x34));
        public int Unknown3C => M.Read<int>(Address + 0x3C); 
        public HeistJob RequiredJob => TheGame.Files.HeistJobs.GetByAddress(M.Read<long>(Address + 0x44, 0x08));
        public int Unknown4C => M.Read<int>(Address + 0x4C);

        public override string ToString()
        {
            return Name;
        }
    }
}