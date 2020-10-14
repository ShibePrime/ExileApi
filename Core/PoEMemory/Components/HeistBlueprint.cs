using ExileCore.PoEMemory.MemoryObjects.Heist;
using ExileCore.Shared.Cache;
using GameOffsets;
using GameOffsets.Native;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.Components
{
    public class HeistBlueprint : Component
    {
        private readonly CachedValue<HeistBlueprintComponentOffsets> _CachedBlueprint;

        public HeistBlueprint()
        {
            _CachedBlueprint =
                new FrameCache<HeistBlueprintComponentOffsets>(() => M.Read<HeistBlueprintComponentOffsets>(Address));
        }

        public HeistBlueprintComponentOffsets BlueprintStruct => _CachedBlueprint.Value;
        public byte AreaLevel => BlueprintStruct.AreaLevel;
        public List<Wing> Wings => GetWings(BlueprintStruct.Wings);

        private List<Wing> GetWings(NativePtrArray source)
        {
            var wings = new List<Wing>();
            var recordSize = HeistBlueprintComponentOffsets.WingRecordSize;

            for (var wingAddress = source.First; wingAddress < source.Last; wingAddress += recordSize)
            {
                wings.Add(GetObject<Wing>(wingAddress));
            }

            return wings;
        }

        public class Wing : RemoteMemoryObject
        {
            public List<HeistJobRecord> Jobs => GetJobs(Address + 0x00);
            public List<HeistChestRewardTypeRecord> RewardRooms => GetRooms(Address + 0x20);
            public List<HeistNpcRecord> Crew => GetCrew(Address + 0x38);

            private List<HeistJobRecord> GetJobs(long source)
            {
                var jobs = new List<HeistJobRecord>();

                var first = M.Read<long>(source);
                var last = M.Read<long>(source + 0x08);
                var recordSize = HeistBlueprintComponentOffsets.JobRecordSize;

                for (var recordAddress = first; recordAddress < last; recordAddress += recordSize)
                {
                    var jobAddress = M.Read<long>(Address + 0x08);
                    if (jobAddress == 0) continue;
                    jobs.Add(TheGame.Files.HeistJobs.GetByAddress(jobAddress));
                }

                return jobs;
            }

            private List<HeistChestRewardTypeRecord> GetRooms(long source)
            {
                var rooms = new List<HeistChestRewardTypeRecord>();

                var first = M.Read<long>(source);
                var last = M.Read<long>(source + 0x08);
                var recordSize = HeistBlueprintComponentOffsets.RoomRecordSize;

                for (var recordAddress = first; recordAddress < last; recordAddress += recordSize)
                {
                    var rewardTypeAddress = M.Read<long>(recordAddress + 0x08);
                    if (rewardTypeAddress == 0) continue;
                    rooms.Add(TheGame.Files.HeistChestRewardType.GetByAddress(rewardTypeAddress));
                }

                return rooms;
            }

            private List<HeistNpcRecord> GetCrew(long source)
            {
                var crew = new List<HeistNpcRecord>();

                var first = M.Read<long>(source);
                var last = M.Read<long>(source + 0x08);
                var recordSize = HeistBlueprintComponentOffsets.NpcRecordSize;

                for (var recordAddress = first; recordAddress < last; recordAddress += recordSize)
                {
                    var npcAddress = M.Read<long>(recordAddress + 0x08);
                    if (npcAddress == 0) continue;
                    crew.Add(TheGame.Files.HeistNpcs.GetByAddress(npcAddress));
                }

                return crew;
            }
        }
    }
}