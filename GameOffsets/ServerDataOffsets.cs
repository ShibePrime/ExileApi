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
        [FieldOffset(0x170)] public NativePtrArray PassiveSkillIds;
        //[FieldOffset(0x188)] public NativePtrArray PassiveBasicJewelNodeIds;
        //[FieldOffset(0x1B8)] public NativePtrArray PassiveClusterJewelNodeIds;
        //[FieldOffset(0x1D0)] public NativePtrArray PassiveMasteryNodeIds;
        [FieldOffset(0x238)] public byte PlayerClass;
        [FieldOffset(0x23C)] public int CharacterLevel;
        [FieldOffset(0x240)] public int PassiveRefundPointsLeft;
        [FieldOffset(0x244)] public int QuestPassiveSkillPoints;
        [FieldOffset(0x248)] public int FreePassiveSkillPointsLeft;
        [FieldOffset(0x24C)] public int TotalAscendencyPoints;
        [FieldOffset(0x250)] public int SpentAscendencyPoints;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ServerDataOffsets
    {
        public const int Skip = 0x8000;
        
        [FieldOffset(0x8820 - Skip)] public long PlayerRelatedData;
        [FieldOffset(0x8838 - Skip)] public byte NetworkState;
        [FieldOffset(0x8850 - Skip)] public NativeStringU League;
        [FieldOffset(0x88D0 - Skip)] public int TimeInGame;
        [FieldOffset(0x88E8 - Skip)] public int Latency;
        [FieldOffset(0x88F0 - Skip)] public NativePtrArray PlayerStashTabs;
        [FieldOffset(0x8908 - Skip)] public NativePtrArray GuildStashTabs;
        [FieldOffset(0x8550 - Skip)] public long FriendsList;
        //[FieldOffset(0x8430 - Skip)] public long FriendNoteList;
        //[FieldOffset(0x8440 - Skip)] public NativePtrArray FriendsArray;
        [FieldOffset(0x8A40 - Skip)] public NativePtrArray PendingInvites;
        [FieldOffset(0x8AB0 - Skip)] public byte PartyStatusType;
        [FieldOffset(0x8AC0 - Skip)] public byte PartyAllocationType; //Incorrect as of 3.16.2b
        [FieldOffset(0x8AC4 - Skip)] public bool PartyDownscaleDisabled; //Incorrect as of 3.16.2b
        [FieldOffset(0x8AC8 - Skip)] public NativePtrArray CurrentParty;
        //[FieldOffset(0x8538 - Skip)] public long GuildList;
        ////[FieldOffset(0x8098 - Skip)] public NativePtrArray GuildArray;
        [FieldOffset(0x8B48 - Skip)] public long GuildNameAddress;
        [FieldOffset(0x8B50 - Skip)] public SkillBarIdsStruct SkillBarIds;
        [FieldOffset(0x8BB0 - Skip)] public NativePtrArray NearestPlayers;
        ////[FieldOffset(0x8AA0 - Skip)] public NativePtrArray MinimapIcons;
        ////[FieldOffset(0x8AC0 - Skip)] public NativePtrArray LocalPlayer;
        [FieldOffset(0x8EB0 - Skip)] public NativePtrArray PlayerInventories; // 3.17.1
        [FieldOffset(0x8E78 - Skip)] public NativePtrArray NPCInventories;
        [FieldOffset(0x8FC8 - Skip)] public NativePtrArray GuildInventories;
        [FieldOffset(0x9138 - Skip)] public ushort TradeChatChannel;
        [FieldOffset(0x9140 - Skip)] public ushort GlobalChatChannel;
        [FieldOffset(0x91E0 - Skip)] public ushort LastActionId;

        //[FieldOffset(0x90C0 - Skip)] public long InstanceLeagueInfo;

        // Note: Search for a LONG value equal to your current amount of completed maps.
        //       Previous byte is a linked list of maps. Map list will be the next byte.
        [FieldOffset(0x9238 - Skip)] public long MavenMapsList;
        [FieldOffset(0x9240 - Skip)] public long MavenMapsCount;
        [FieldOffset(0x9248 - Skip)] public long MavenMapsArray;
        [FieldOffset(0x9278 - Skip)] public long CompletedMapsList;
        [FieldOffset(0x9280 - Skip)] public long CompletedMapsCount;
        [FieldOffset(0x9288 - Skip)] public long CompletedMapsArray;
        [FieldOffset(0x92B8 - Skip)] public long BonusCompletedAreasList;
        [FieldOffset(0x92C0 - Skip)] public long BonusCompletedAreasCount;
        [FieldOffset(0x92C8 - Skip)] public long BonusCompletedAreasArray;
        [FieldOffset(0x92F8 - Skip)] public long AwakenedAreasList;
        [FieldOffset(0x9300 - Skip)] public long AwakenedAreasCount;
        [FieldOffset(0x9308 - Skip)] public long AwakenedAreasArray;
        ////[FieldOffset(0x8A08 - Skip)] public long BestiaryCapturedMonsterList;

        public const int AtlasRegionUpgrades = 0x92BA;
        public const int AtlasWatchtowerLocations = 0x92C0;
        public const int BestiaryBeastsCapturedCounts = 0x8AE8; //

        [FieldOffset(0x9C98 - Skip)] public int DialogDepth;
        [FieldOffset(0x9C9C - Skip)] public byte MonsterLevel;
        [FieldOffset(0x9C9D - Skip)] public byte MonstersRemaining;
        //[FieldOffset(0x9328 - Skip)] public long LeftIncursionArchitectKey;
        //[FieldOffset(0x9338 - Skip)] public long RightIncursionArchitectKey;
        [FieldOffset(0x9D18 - Skip)] public int CurrentAzuriteAmount;
        [FieldOffset(0x9D28 - Skip)] public ushort CurrentSulphiteAmount;

        //[FieldOffset(0x9590 - Skip)] public long DeliriumRewardInfo;
        //[FieldOffset(0x95A8 - Skip)] public long BlueprintRevealInfo;
    }
}
