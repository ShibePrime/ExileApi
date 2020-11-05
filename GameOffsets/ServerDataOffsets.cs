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
        public const int AtlasRegionUpgrades = 0x819A;
        public const int AtlasWatchtowerLocations = 0x81A8;
        public const int BestiaryBeastsCapturedCounts = 0x8378; // TODO: Changed in 3.12.4D

        [FieldOffset(0x7818 - Skip)] public long PlayerRelatedData;
        [FieldOffset(0x7828 - Skip)] public byte NetworkState;
        [FieldOffset(0x7840 - Skip)] public NativeStringU League;
        [FieldOffset(0x78C0 - Skip)] public float TimeInGame;
        [FieldOffset(0x78C8 - Skip)] public int Latency;
        [FieldOffset(0x78D8 - Skip)] public NativePtrArray PlayerStashTabs;
        [FieldOffset(0x78F0 - Skip)] public NativePtrArray GuildStashTabs;
        //[FieldOffset(0x7918 - Skip)] public NativePtrArray GuildList; // TODO: Changed in 3.12.4D
        //[FieldOffset(0x7930 - Skip)] public NativePtrArray FriendsList; // TODO: Changed in 3.12.4D
        //[FieldOffset(0x7940 - Skip)] public NativePtrArray UnknownSocialList; // TODO: Changed in 3.12.4D
        //[FieldOffset(0x7980 - Skip)] public NativePtrArray PendingInvites; // TODO: Changed in 3.12.4D
        [FieldOffset(0x7A00 - Skip)] public byte PartyStatusType; // TODO: Changed in 3.12.4D
        //[FieldOffset(0x7A08 - Skip)] public NativePtrArray CurrentParty; // TODO: Changed in 3.12.4D
        [FieldOffset(0x7A20 - Skip)] public byte PartyAllocationType; // TODO: Changed in 3.12.4D
        [FieldOffset(0x7AC8 - Skip)] public long GuildNameAddress;
        [FieldOffset(0x7AD0 - Skip)] public SkillBarIdsStruct SkillBarIds;
        [FieldOffset(0x7B28 - Skip)] public NativePtrArray NearestPlayers;
        [FieldOffset(0x7C68 - Skip)] public NativePtrArray PlayerInventories;
        [FieldOffset(0x7D78 - Skip)] public NativePtrArray NPCInventories;
        [FieldOffset(0x7E88 - Skip)] public NativePtrArray GuildInventories;
        [FieldOffset(0x7FC8 - Skip)] public ushort TradeChatChannel;
        [FieldOffset(0x7FD0 - Skip)] public ushort GlobalChatChannel;
        [FieldOffset(0x8020 - Skip)] public ushort LastActionId;
        
        // Note: Search for a LONG value equal to your current amount of completed maps. Map list will be the next byte.
        [FieldOffset(0x80A0 - Skip)] public long CompletedMaps; 
        [FieldOffset(0x80E0 - Skip)] public long BonusCompletedAreas;
        [FieldOffset(0x8120 - Skip)] public long AwakenedAreas;
        [FieldOffset(0x8BD4 - Skip)] public byte MonsterLevel;
        [FieldOffset(0x8BD5 - Skip)] public byte MonstersRemaining;
        [FieldOffset(0x8C78 - Skip)] public int CurrentAzuriteAmount;
        [FieldOffset(0x8C88 - Skip)] public ushort CurrentSulphiteAmount;
    }
}
