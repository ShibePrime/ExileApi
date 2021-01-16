using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SkillBarIdsStruct
    {
        public ushort SkillBar1;
        public ushort SkillBar2;
        public ushort SkillBar3;
        public ushort SkillBar4;
        public ushort SkillBar5;
        public ushort SkillBar6;
        public ushort SkillBar7;
        public ushort SkillBar8;
        public ushort SkillBar9;
        public ushort SkillBar10;
        public ushort SkillBar11;
        public ushort SkillBar12;
        public ushort SkillBar13;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ServerPlayerDataOffsets
    {
        [FieldOffset(0x160)] public NativePtrArray PassiveSkillIds;
        //[FieldOffset(0x178)] public NativePtrArray PassiveJewelSocketIds;
        [FieldOffset(0x200)] public byte PlayerClass;
        [FieldOffset(0x204)] public int CharacterLevel;
        [FieldOffset(0x208)] public int PassiveRefundPointsLeft;
        [FieldOffset(0x20C)] public int QuestPassiveSkillPoints;
        [FieldOffset(0x210)] public int FreePassiveSkillPointsLeft;
        [FieldOffset(0x214)] public int TotalAscendencyPoints;
        [FieldOffset(0x218)] public int SpentAscendencyPoints;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ServerDataOffsets
    {
        public const int Skip = 0x7000;
        public const int AtlasRegionUpgrades = 0x821A;
        public const int AtlasWatchtowerLocations = 0x8228;
        public const int BestiaryBeastsCapturedCounts = 0x8438;

        [FieldOffset(0x7B18 - Skip)] public long PlayerRelatedData;
        [FieldOffset(0x7BB8 - Skip)] public byte NetworkState;
        [FieldOffset(0x7BD0 - Skip)] public NativeStringU League;
        [FieldOffset(0x7C50 - Skip)] public float TimeInGame;
        [FieldOffset(0x7C58 - Skip)] public int Latency;
        [FieldOffset(0x7C68 - Skip)] public NativePtrArray PlayerStashTabs;
        [FieldOffset(0x7C80 - Skip)] public NativePtrArray GuildStashTabs;
        [FieldOffset(0x7CE8 - Skip)] public NativePtrArray GuildList;
        [FieldOffset(0x7D00 - Skip)] public NativePtrArray FriendsList;
        [FieldOffset(0x7D50 - Skip)] public NativePtrArray PendingInvites;
        [FieldOffset(0x7DD0 - Skip)] public byte PartyStatusType;
        [FieldOffset(0x7DD8 - Skip)] public NativePtrArray CurrentParty;
        [FieldOffset(0x7DF0 - Skip)] public byte PartyAllocationType;
        [FieldOffset(0x7DF1 - Skip)] public bool PartyDownscaleDisabled;
        [FieldOffset(0x7E58 - Skip)] public long GuildNameAddress;
        [FieldOffset(0x7E60 - Skip)] public SkillBarIdsStruct SkillBarIds;
        [FieldOffset(0x7EB8 - Skip)] public NativePtrArray NearestPlayers;
        [FieldOffset(0x7FF8 - Skip)] public NativePtrArray PlayerInventories;
        [FieldOffset(0x8120 - Skip)] public NativePtrArray NPCInventories;
        [FieldOffset(0x7F08 - Skip)] public NativePtrArray GuildInventories;
        [FieldOffset(0x8428 - Skip)] public ushort TradeChatChannel;
        [FieldOffset(0x8380 - Skip)] public ushort GlobalChatChannel;
        [FieldOffset(0x83E0 - Skip)] public ushort LastActionId;
        
        // Note: Search for a LONG value equal to your current amount of completed maps. Map list will be the next byte.
        [FieldOffset(0x84D8 - Skip)] public long CompletedMaps; 
        [FieldOffset(0x8498 - Skip)] public long BonusCompletedAreas;
        [FieldOffset(0x8518 - Skip)] public long AwakenedAreas;
        [FieldOffset(0x8FCC - Skip)] public byte MonsterLevel;
        [FieldOffset(0x8FCD - Skip)] public byte MonstersRemaining;
        [FieldOffset(0x9070 - Skip)] public int CurrentAzuriteAmount;
        [FieldOffset(0x9080 - Skip)] public ushort CurrentSulphiteAmount;
    }
}
