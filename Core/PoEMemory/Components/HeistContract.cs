using ExileCore.PoEMemory.MemoryObjects.Heist;
using ExileCore.PoEMemory.Models;
using ExileCore.Shared.Cache;
using GameOffsets;

namespace ExileCore.PoEMemory.Components
{
    public class HeistContract : Component
    {
        private readonly CachedValue<HeistContractComponentOffsets> _ContractData;
        private readonly CachedValue<HeistContractObjectiveOffsets> _ObjectivesData;
        private readonly CachedValue<HeistContractRequirementOffsets> _RequirementData;

        public HeistContract()
        {
            _ContractData = new FrameCache<HeistContractComponentOffsets>(() =>
                M.Read<HeistContractComponentOffsets>(Address));
            _ObjectivesData = new FrameCache<HeistContractObjectiveOffsets>(() =>
                M.Read<HeistContractObjectiveOffsets>(_ContractData.Value.ObjectiveKey));
            _RequirementData = new FrameCache<HeistContractRequirementOffsets>(() =>
                M.Read<HeistContractRequirementOffsets>(_ContractData.Value.Requirements.First));
        }

        private HeistContractObjectiveOffsets Objectives => _ObjectivesData.Value;
        private HeistContractRequirementOffsets Requirements => _RequirementData.Value;

        public BaseItemType TargetItem => TheGame.Files.BaseItemTypes.GetFromAddress(Objectives.TargetKey);
        public string Client => M.ReadStringU(Objectives.ClientKey);
        public HeistJobRecord RequiredJob => TheGame.Files.HeistJobs.GetByAddress(Requirements.JobKey);
        public byte RequiredJobLevel => Requirements.JobLevel;
    }
}