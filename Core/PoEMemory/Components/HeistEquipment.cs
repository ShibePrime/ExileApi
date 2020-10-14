using ExileCore.PoEMemory.MemoryObjects.Heist;
using ExileCore.PoEMemory.Models;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using GameOffsets;
using System;

namespace ExileCore.PoEMemory.Components
{
    public class HeistEquipment : Component
    {
        private readonly Lazy<HeistEquipmentOffsets> _HeistEquipmentItem;
        private readonly CachedValue<BaseItemType> _ItemBase;
        private readonly CachedValue<HeistJobRecord> _Job;

        public HeistEquipment()
        {
            var component = new Lazy<HeistEquipmentComponentOffsets>(
                () => M.Read<HeistEquipmentComponentOffsets>(Address));
            var componentData = new Lazy<HeistEquipmentComponentDataOffsets>(
                () => M.Read<HeistEquipmentComponentDataOffsets>(component.Value.DataKey));

            _HeistEquipmentItem = new Lazy<HeistEquipmentOffsets>(
                () => M.Read<HeistEquipmentOffsets>(componentData.Value.HeistEquipmentKey));
            _ItemBase = new StaticValueCache<BaseItemType>(
                () => TheGame.Files.BaseItemTypes.GetFromAddress(_HeistEquipmentItem.Value.BaseItemKey));
            _Job = new StaticValueCache<HeistJobRecord>(
                () => TheGame.Files.HeistJobs.GetByAddress(_HeistEquipmentItem.Value.RequiredJobKey));
        }

        public BaseItemType ItemBase => _ItemBase.Value;
        public HeistJobRecord RequiredJob => _Job.Value;
        public int JobMinimumLevel => _HeistEquipmentItem.Value.RequiredJobMinimumLevel;

        public HeistJobE RequiredJobE => _Job.Value == null
            ? HeistJobE.Any
            : (HeistJobE) TheGame.Files.HeistJobs.EntriesList.FindIndex(
                job => job.Address == _HeistEquipmentItem.Value.RequiredJobKey);
    }
}