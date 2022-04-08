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
        [FieldOffset(0x180)] public NativePtrArray PassiveSkillIds; // 3.17.3.3
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
        // 0x8A50-0x8A58 is constantly counting up (time?)
        // 0x8A10-0x820, 0x8AC0, 0x8B00, 0x8B88, 0x8C90, 0x8D28, 0x8D70, 0x8D78, 0x8EC0 seems to point to objects/collections with data
        // 0x8D00 mouse pos?
        public const int Skip = 0x8000;
        public const int AtlasRegionUpgrades = 0x92BA;
        public const int AtlasWatchtowerLocations = 0x92C0;
        public const int BestiaryBeastsCapturedCounts = 0x8AE8;


        [FieldOffset(0x89A0 - Skip)] public long PlayerRelatedData;

        [FieldOffset(0x89B8 - Skip)] public byte NetworkState;

        [FieldOffset(0x89E0 - Skip)] public NativeStringU League;

        [FieldOffset(0x8A08 - Skip)] public byte PartyAllocationType;

        [FieldOffset(0x8A68 - Skip)] public int Latency;

        [FieldOffset(0x8A78 - Skip)] public NativePtrArray PlayerStashTabs;

        [FieldOffset(0x8C48 - Skip)] public byte PartyStatusType;

        [FieldOffset(0x8CD0 - Skip)] public long GuildNameAddress;

        [FieldOffset(0x8CE0 - Skip)] public SkillBarIdsStruct SkillBarIds;

        [FieldOffset(0x8DA8 - Skip)] public long MavenMapsCount; // May be incorrect

        [FieldOffset(0x8EB8 - Skip)] public NativePtrArray PlayerInventories;

        [FieldOffset(0x9178 - Skip)] public int SearingExtractCount; // Maybe be incorrect

        [FieldOffset(0x91F0 - Skip)] public ushort LastActionId;

        [FieldOffset(0x92C8 - Skip)] public ushort TradeChatChannel;
        [FieldOffset(0x92D0 - Skip)] public ushort GlobalChatChannel;

        // incorrect 0x9090
        [FieldOffset(0x92D8 - Skip)] public byte MonsterLevel; // Area level right? (I believe offset was somewhere arround 0x92C8 
        [FieldOffset(0x92D8 - Skip)] public NativePtrArray NPCInventories; // Does this even exist anymore?
        [FieldOffset(0x92D8 - Skip)] public int CurrentAzuriteAmount; // incorrect
        [FieldOffset(0x92D8 - Skip)] public ushort CurrentSulphiteAmount; // incorrect
        [FieldOffset(0x92D8 - Skip)] public NativePtrArray NearestPlayers; // incorrect
        [FieldOffset(0x92D8 - Skip)] public NativePtrArray GuildStashTabs; // incorrect
        [FieldOffset(0x92D8 - Skip)] public NativePtrArray GuildInventories; // incorrect
        [FieldOffset(0x92D8 - Skip)] public long BonusCompletedAreasList; // incorrect
        [FieldOffset(0x92D8 - Skip)] public long MavenMapsList; // incorrect
        [FieldOffset(0x92D8 - Skip)] public long MavenMapsArray; // incorrect

        [FieldOffset(0x92D8 - Skip)] public long CompletedMapsList; // incorrect
        [FieldOffset(0x92D8 - Skip)] public long CompletedMapsCount; // incorrect
        [FieldOffset(0x92D8 - Skip)] public long CompletedMapsArray; // incorrect
        [FieldOffset(0x92D8 - Skip)] public long AwakenedAreasList; // incorrect
        [FieldOffset(0x92D8 - Skip)] public long AwakenedAreasCount; // incorrect
        [FieldOffset(0x92D8 - Skip)] public long AwakenedAreasArray; // incorrect

        [FieldOffset(0x92D8 - Skip)] public int DialogDepth; // incorrect
        [FieldOffset(0x92D8 - Skip)] public byte MonstersRemaining; // incorrect

        //[FieldOffset(0x90C0 - Skip)] public long InstanceLeagueInfo;
        //[FieldOffset(0x8AA0 - Skip + 8)] public NativePtrArray MinimapIcons;
        //[FieldOffset(0x8AB0 - Skip + 0x8)] public byte PartyStatusType;
        //[FieldOffset(0x8AC0 - Skip + 8)] public NativePtrArray LocalPlayer;
        //[FieldOffset(0x8AA0 - Skip + 8)] public NativePtrArray MinimapIcons;
        //[FieldOffset(0x8AC4 - Skip + 8)] public bool PartyDownscaleDisabled;
        //[FieldOffset(0x8AC8 - Skip + 0x8)] public NativePtrArray CurrentParty;
        //FieldOffset(0x92C0 - Skip + 8)] public long BonusCompletedAreasCount;
        //[FieldOffset(0x92C8 - Skip + 8)] public long BonusCompletedAreasArray;
        //[FieldOffset(0x9590 - Skip + 8)] public long DeliriumRewardInfo;
        //[FieldOffset(0x95A8 - Skip + 8)] public long BlueprintRevealInfo; 
        //[FieldOffset(0x9328 - Skip + 8)] public long LeftIncursionArchitectKey;
        //[FieldOffset(0x9338 - Skip + 8)] public long RightIncursionArchitectKey;
        //[FieldOffset(0x8098 - Skip)] public NativePtrArray GuildArray;
        //[FieldOffset(0x8430 - Skip)] public long FriendNoteList;
        //[FieldOffset(0x8440 - Skip)] public NativePtrArray FriendsArray;
        //[FieldOffset(0x8538 - Skip)] public long GuildList;
        //[FieldOffset(0x8550 - Skip)] public long FriendsList;
        //[FieldOffset(0x8A08 - Skip)] public long BestiaryCapturedMonsterList;
        //[FieldOffset(0x8A40 - Skip)] public NativePtrArray PendingInvites;
        //[FieldOffset(0x8A50 - Skip)] public int TimeInGame;
    }
}
