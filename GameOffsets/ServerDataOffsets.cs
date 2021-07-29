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
        //[FieldOffset(0x178)] public NativePtrArray PassiveJewelSocketIds;
        [FieldOffset(0x210)] public byte PlayerClass;
        [FieldOffset(0x214)] public int CharacterLevel;
        [FieldOffset(0x218)] public int PassiveRefundPointsLeft;
        [FieldOffset(0x21C)] public int QuestPassiveSkillPoints;
        [FieldOffset(0x220)] public int FreePassiveSkillPointsLeft;
        [FieldOffset(0x224)] public int TotalAscendencyPoints;
        [FieldOffset(0x228)] public int SpentAscendencyPoints;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ServerDataOffsets
    {
        public const int Skip = 0x7000;

        [FieldOffset(0x8218 - Skip)] public long PlayerRelatedData;
        [FieldOffset(0x82B8 - Skip)] public byte NetworkState;
        [FieldOffset(0x82D0 - Skip)] public NativeStringU League;
        [FieldOffset(0x8350 - Skip)] public float TimeInGame;
        [FieldOffset(0x8368 - Skip)] public int Latency;
        [FieldOffset(0x8370 - Skip)] public NativePtrArray PlayerStashTabs;
        [FieldOffset(0x8388 - Skip)] public NativePtrArray GuildStashTabs;
        [FieldOffset(0x8418 - Skip)] public long FriendsList;
        //[FieldOffset(0x8430 - Skip)] public long FriendNoteList;
        //[FieldOffset(0x8440 - Skip)] public NativePtrArray FriendsArray;
        [FieldOffset(0x8480 - Skip)] public NativePtrArray PendingInvites;
        [FieldOffset(0x8500 - Skip)] public byte PartyStatusType;
        [FieldOffset(0x8508 - Skip)] public NativePtrArray CurrentParty;
        [FieldOffset(0x8520 - Skip)] public byte PartyAllocationType;
        [FieldOffset(0x8521 - Skip)] public bool PartyDownscaleDisabled;
        [FieldOffset(0x8538 - Skip)] public long GuildList;
        //[FieldOffset(0x8098 - Skip)] public NativePtrArray GuildArray;
        [FieldOffset(0x8588 - Skip)] public long GuildNameAddress;
        [FieldOffset(0x8590 - Skip)] public SkillBarIdsStruct SkillBarIds;
        [FieldOffset(0x85E8 - Skip)] public NativePtrArray NearestPlayers;
        //[FieldOffset(0x8168 - Skip)] public NativePtrArray MinimapIcons;
        //[FieldOffset(0x8188 - Skip)] public NativePtrArray LocalPlayer;
        [FieldOffset(0x8758 - Skip)] public NativePtrArray PlayerInventories;
        [FieldOffset(0x8898 - Skip)] public NativePtrArray NPCInventories;
        [FieldOffset(0x89D8 - Skip)] public NativePtrArray GuildInventories;

        [FieldOffset(0x8B40 - Skip)] public ushort TradeChatChannel;
        [FieldOffset(0x8B48 - Skip)] public ushort GlobalChatChannel;
        [FieldOffset(0x8BE8 - Skip)] public ushort LastActionId;

        //[FieldOffset(0x8708 - Skip)] public long InstanceLeagueInfo;

        // Note: Search for a LONG value equal to your current amount of completed maps.
        //       Previous byte is a linked list of maps. Map list will be the next byte.
        [FieldOffset(0x8C40 - Skip)] public long MavenMapsList;
        [FieldOffset(0x8C48 - Skip)] public long MavenMapsCount;
        [FieldOffset(0x8C50 - Skip)] public long MavenMapsArray;
        [FieldOffset(0x8C80 - Skip)] public long CompletedMapsList;
        [FieldOffset(0x8C88 - Skip)] public long CompletedMapsCount;
        [FieldOffset(0x8C90 - Skip)] public long CompletedMapsArray;
        [FieldOffset(0x8CC0 - Skip)] public long BonusCompletedAreasList;
        [FieldOffset(0x8CC8 - Skip)] public long BonusCompletedAreasCount;
        [FieldOffset(0x8CD0 - Skip)] public long BonusCompletedAreasArray;
        [FieldOffset(0x8D00 - Skip)] public long AwakenedAreasList;
        [FieldOffset(0x8D08 - Skip)] public long AwakenedAreasCount;
        [FieldOffset(0x8D10 - Skip)] public long AwakenedAreasArray;
        //[FieldOffset(0x8A08 - Skip)] public long BestiaryCapturedMonsterList;

        public const int AtlasRegionUpgrades = 0x8D8A;
        public const int AtlasWatchtowerLocations = 0x8D98;
        public const int BestiaryBeastsCapturedCounts = 0x8A68;

        [FieldOffset(0x9770 - Skip)] public int DialogDepth;
        [FieldOffset(0x9774 - Skip)] public byte MonsterLevel;
        [FieldOffset(0x9775 - Skip)] public byte MonstersRemaining;
        //[FieldOffset(0x9328 - Skip)] public long LeftIncursionArchitectKey;
        //[FieldOffset(0x9338 - Skip)] public long RightIncursionArchitectKey;
        [FieldOffset(0x9820 - Skip)] public int CurrentAzuriteAmount;
        [FieldOffset(0x9830 - Skip)] public ushort CurrentSulphiteAmount;

        //[FieldOffset(0x9590 - Skip)] public long DeliriumRewardInfo;
        //[FieldOffset(0x95A8 - Skip)] public long BlueprintRevealInfo;
    }
}
