using ExileCore.PoEMemory.FilesInMemory.Heist;
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
            public List<HeistJob> Jobs => GetJobs(Address + 0x00);
            public List<HeistChestRewardType> RewardRooms => GetRooms(Address + 0x20);
            public List<HeistNpc> Crew => GetCrew(Address + 0x38);

            private List<HeistJob> GetJobs(long source)
            {
                var jobs = new List<HeistJob>();

                var first = M.Read<long>(source);
                var last = M.Read<long>(source + 0x08);
                var recordSize = HeistBlueprintComponentOffsets.JobRecordSize;

                for (var recordAddress = first; recordAddress < last; recordAddress += recordSize)
                {
                    jobs.Add(TheGame.Files.HeistJobs.GetByAddress(M.Read<long>(recordAddress + 0x08)));
                }

                return jobs;
            }

            private List<HeistChestRewardType> GetRooms(long source)
            {
                var rooms = new List<HeistChestRewardType>();

                var first = M.Read<long>(source);
                var last = M.Read<long>(source + 0x08);
                var recordSize = HeistBlueprintComponentOffsets.RoomRecordSize;

                for (var recordAddress = first; recordAddress < last; recordAddress += recordSize)
                {
                    rooms.Add(TheGame.Files.HeistChestRewardType.GetByAddress(M.Read<long>(recordAddress + 0x08)));
                }

                return rooms;
            }

            private List<HeistNpc> GetCrew(long source)
            {
                var crew = new List<HeistNpc>();

                var first = M.Read<long>(source);
                var last = M.Read<long>(source + 0x08);
                var recordSize = HeistBlueprintComponentOffsets.NpcRecordSize;

                for (var recordAddress = first; recordAddress < last; recordAddress += recordSize)
                {
                    crew.Add(TheGame.Files.HeistNpcs.GetByAddress(M.Read<long>(recordAddress + 0x08)));
                }

                return crew;
            }
        }
    }
}