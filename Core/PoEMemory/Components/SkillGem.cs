using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using GameOffsets;

namespace ExileCore.PoEMemory.Components
{
    public class SkillGem : Component
    {
        private readonly CachedValue<SkillGemOffsets> _Info;
        private readonly FrameCache<GemInformation> _AdvancedInfo;

        public SkillGem()
        {
            _Info = new FrameCache<SkillGemOffsets>(() => M.Read<SkillGemOffsets>(Address));
            _AdvancedInfo = new FrameCache<GemInformation>(() => M.Read<GemInformation>(_Info.Value.AdvanceInformation));
        }

        public int Level => (int)_Info.Value.Level;//TODO: fixme, remove cast
        public uint TotalExpGained => _Info.Value.TotalExpGained;
        public uint ExperiencePrevLevel => _Info.Value.TotalExpGained;
        public uint ExperienceMaxLevel => _Info.Value.ExperienceMaxLevel;
        public uint ExperienceToNextLevel => ExperienceMaxLevel - ExperiencePrevLevel;
        public int MaxLevel => _AdvancedInfo.Value.MaxLevel;
        public int SocketColor => _AdvancedInfo.Value.SocketColor;
        public SkillGemQualityTypeE QualityType => (SkillGemQualityTypeE) _Info.Value.QualityTypeId;
        public bool HasAlternateQualityType => QualityType != SkillGemQualityTypeE.Superior;
    }
}
