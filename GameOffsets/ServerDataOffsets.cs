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

        [FieldOffset(0x7D18 - Skip)] public long PlayerRelatedData;
        [FieldOffset(0x7DB8 - Skip)] public byte NetworkState;
        [FieldOffset(0x7DD0 - Skip)] public NativeStringU League;
        [FieldOffset(0x7E50 - Skip)] public float TimeInGame;
        [FieldOffset(0x7E58 - Skip)] public int Latency;
        [FieldOffset(0x7E68 - Skip)] public NativePtrArray PlayerStashTabs;
        [FieldOffset(0x7E80 - Skip)] public NativePtrArray GuildStashTabs;
        [FieldOffset(0x7EE8 - Skip)] public long FriendsList;
        //[FieldOffset(0x7F00 - Skip)] public long FriendNoteList;
        //[FieldOffset(0x7F10 - Skip)] public NativePtrArray FriendsArray;
        [FieldOffset(0x7F50 - Skip)] public NativePtrArray PendingInvites;
        [FieldOffset(0x7FD0 - Skip)] public byte PartyStatusType;
        [FieldOffset(0x7FD8 - Skip)] public NativePtrArray CurrentParty;
        [FieldOffset(0x7FF0 - Skip)] public byte PartyAllocationType;
        [FieldOffset(0x7FF1 - Skip)] public bool PartyDownscaleDisabled;
        [FieldOffset(0x8008 - Skip)] public long GuildList;
        //[FieldOffset(0x8018 - Skip)] public NativePtrArray GuildArray;
        [FieldOffset(0x8058 - Skip)] public long GuildNameAddress;
        [FieldOffset(0x8060 - Skip)] public SkillBarIdsStruct SkillBarIds;
        [FieldOffset(0x80B8 - Skip)] public NativePtrArray NearestPlayers;
        //[FieldOffset(0x80E8 - Skip)] public NativePtrArray MinimapIcons;
        //[FieldOffset(0x8108 - Skip)] public NativePtrArray LocalPlayer;
        [FieldOffset(0x8220 - Skip)] public NativePtrArray PlayerInventories;
        [FieldOffset(0x8358 - Skip)] public NativePtrArray NPCInventories;
        [FieldOffset(0x8490 - Skip)] public NativePtrArray GuildInventories;

        [FieldOffset(0x85F8 - Skip)] public ushort TradeChatChannel;
        [FieldOffset(0x8600 - Skip)] public ushort GlobalChatChannel;
        [FieldOffset(0x8650 - Skip)] public ushort LastActionId;
        
        // Note: Search for a LONG value equal to your current amount of completed maps.
        //       Previous byte is a linked list of maps. Map list will be the next byte.
        [FieldOffset(0x86A8 - Skip)] public long MavenMapsList;
        [FieldOffset(0x86B0 - Skip)] public long MavenMapsCount;
        [FieldOffset(0x86B8 - Skip)] public long MavenMapsArray;
        [FieldOffset(0x86E8 - Skip)] public long CompletedMapsList;
        [FieldOffset(0x86F0 - Skip)] public long CompletedMapsCount;
        [FieldOffset(0x86F8 - Skip)] public long CompletedMapsArray;
        [FieldOffset(0x8728 - Skip)] public long BonusCompletedAreasList;
        [FieldOffset(0x8730 - Skip)] public long BonusCompletedAreasCount;
        [FieldOffset(0x8738 - Skip)] public long BonusCompletedAreasArray;
        [FieldOffset(0x8768 - Skip)] public long AwakenedAreasList;
        [FieldOffset(0x8770 - Skip)] public long AwakenedAreasCount;
        [FieldOffset(0x8778 - Skip)] public long AwakenedAreasArray;
        //[FieldOffset(0x8988 - Skip)] public long BestiaryCapturedMonsterList;

        public const int AtlasRegionUpgrades = 0x87F2;
        public const int AtlasWatchtowerLocations = 0x8800;
        public const int BestiaryBeastsCapturedCounts = 0x89E8;

        [FieldOffset(0x922C - Skip)] public byte MonsterLevel;
        [FieldOffset(0x922D - Skip)] public byte MonstersRemaining;
        [FieldOffset(0x92D8 - Skip)] public int CurrentAzuriteAmount;
        [FieldOffset(0x92E8 - Skip)] public ushort CurrentSulphiteAmount;
    }
}
