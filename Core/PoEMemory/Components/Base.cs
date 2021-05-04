using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using GameOffsets.Native;
using GameOffsets;
using ExileCore.Shared.Cache;

namespace ExileCore.PoEMemory.Components
{
    public class Base : Component
    {
        private readonly CachedValue<BaseComponentOffsets> _cachedValue;

        public Base()
        {
            _cachedValue = new FrameCache<BaseComponentOffsets>(() => M.Read<BaseComponentOffsets>(Address));
        }

        public BaseComponentOffsets BaseStruct => _cachedValue.Value;

        //x20 - some strings about item
        private string _name;
        public string Name => _name ?? (_name = M.Read<NativeStringU>(Address + 0x10, 0x18).ToString(M));
        public byte ItemCellsSizeX => M.Read<byte>(Address + 0x10, 0x10);
        public byte ItemCellsSizeY => M.Read<byte>(Address + 0x10, 0x11);
        private Influence InfluenceFlag => (Influence)BaseStruct.InfluenceFlag;
        public bool isShaper => (InfluenceFlag & Influence.Shaper) == Influence.Shaper;
        public bool isElder => (InfluenceFlag & Influence.Elder) == Influence.Elder;
        public bool isCrusader => (InfluenceFlag & Influence.Crusader) == Influence.Crusader;
        public bool isHunter => (InfluenceFlag & Influence.Hunter) == Influence.Hunter;
        public bool isRedeemer => (InfluenceFlag & Influence.Redeemer) == Influence.Redeemer;
        public bool isWarlord => (InfluenceFlag & Influence.Warlord) == Influence.Warlord;
        public bool isCorrupted => (BaseStruct.isCorrupted & 0x01) == 0x01;
        // REVISIT: not using Cache.StringCache here.  no profiles
        // point to it being used often enough in a single frame.
        public string PublicPrice => M.Read<NativeStringU>(BaseStruct.PublicPricePtr).ToString(M);

        // public bool isFractured => M.Read<byte>(Address + 0x98) == 0; // TODO: 3.12.2

        // 0x8 - link to base item
        // +0x10 - Name
        // +0x30 - Use hint
        // +0x50 - Link to Data/BaseItemTypes.dat

        // 0xC (+4) fileref to visual identity
    }
}
