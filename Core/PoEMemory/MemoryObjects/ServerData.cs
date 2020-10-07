using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.FilesInMemory.Atlas;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using GameOffsets;
using GameOffsets.Native;

namespace ExileCore.PoEMemory.MemoryObjects
{
    public class ServerPlayerData : RemoteMemoryObject
    {
        private readonly CachedValue<ServerPlayerDataOffsets> _cachedValue;

        public ServerPlayerData()
        {
            _cachedValue = new FrameCache<ServerPlayerDataOffsets>(() => M.Read<ServerPlayerDataOffsets>(Address));
        }
        public ServerPlayerDataOffsets ServerPlayerDataStruct => _cachedValue.Value;

        public CharacterClass Class => (CharacterClass) (ServerPlayerDataStruct.PlayerClass & 0x0F);
        public int Level => ServerPlayerDataStruct.CharacterLevel;
        public int PassiveRefundPointsLeft => ServerPlayerDataStruct.PassiveRefundPointsLeft;
        public int QuestPassiveSkillPoints => ServerPlayerDataStruct.QuestPassiveSkillPoints;
        public int FreePassiveSkillPointsLeft => ServerPlayerDataStruct.FreePassiveSkillPointsLeft;
        public int TotalAscendencyPoints => ServerPlayerDataStruct.TotalAscendencyPoints;
        public int SpentAscendencyPoints => ServerPlayerDataStruct.SpentAscendencyPoints;
        public NativePtrArray AllocatedPassivesIds => ServerPlayerDataStruct.PassiveSkillIds;
    }
    public class ServerData : RemoteMemoryObject
    {
        private static readonly int NetworkStateOff =
            Extensions.GetOffset<ServerDataOffsets>(nameof(ServerDataOffsets.NetworkState)) + ServerDataOffsets.Skip;

        private readonly CachedValue<ServerDataOffsets> _cachedValue;
        private readonly CachedValue<ServerPlayerData> _playerData;
        private readonly List<Player> result = new List<Player>();

        private const int MaxElementsToScan = 1024;

        public ServerData()
        {
            _cachedValue = new FrameCache<ServerDataOffsets>(() => M.Read<ServerDataOffsets>(Address + ServerDataOffsets.Skip));
            _playerData = new FrameCache<ServerPlayerData>(() => GetObject<ServerPlayerData>(_cachedValue.Value.PlayerRelatedData));
        }

        public ServerDataOffsets ServerDataStruct => _cachedValue.Value;
        public ServerPlayerData PlayerInformation => _playerData.Value;
        public ushort TradeChatChannel => ServerDataStruct.TradeChatChannel;
        public ushort GlobalChatChannel => ServerDataStruct.GlobalChatChannel;
        public byte MonsterLevel => ServerDataStruct.MonsterLevel;
        public CharacterClass PlayerClass => PlayerInformation.Class;

        //if 51 - more than 50 monsters remaining (no exact number)
        //if 255 - not supported for current map (town or scenary map)
        public byte MonstersRemaining => ServerDataStruct.MonstersRemaining;
        public ushort CurrentSulphiteAmount => _cachedValue.Value.CurrentSulphiteAmount;
        public int CurrentAzuriteAmount => _cachedValue.Value.CurrentAzuriteAmount;

        public IList<Player> NearestPlayers
        {
            get
            {
                if (Address == 0)
                {
                    return result;
                }

                const int structSize = 0x18;
                var first = ServerDataStruct.NearestPlayers.First;
                var last = ServerDataStruct.NearestPlayers.Last;

                if (first < 0 || last < 0 || (last - first) / structSize > 64)
                {
                    return result;
                }

                result.Clear();

                for (var playerAddress = first; playerAddress < last; playerAddress += structSize)
                {
                    result.Add(ReadObject<Player>(playerAddress));
                }

                return result;
            }
        }

        public int GetBeastCapturedAmount(BestiaryCapturableMonster monster)
        {
            return M.Read<int>(Address + 0x5240 + monster.Id * 4);
        }

        #region PlayerData

        public ushort LastActionId => ServerDataStruct.LastActionId;
        public int CharacterLevel => PlayerInformation.Level;
        public int PassiveRefundPointsLeft => PlayerInformation.PassiveRefundPointsLeft;
        public int FreePassiveSkillPointsLeft => PlayerInformation.FreePassiveSkillPointsLeft;
        public int QuestPassiveSkillPoints => PlayerInformation.QuestPassiveSkillPoints;
        public int TotalAscendencyPoints => PlayerInformation.TotalAscendencyPoints;
        public int SpentAscendencyPoints => PlayerInformation.SpentAscendencyPoints;
        public PartyAllocation PartyAllocationType => (PartyAllocation) ServerDataStruct.PartyAllocationType;
        public string League => ServerDataStruct.League.ToString(M);
        public PartyStatus PartyStatusType => (PartyStatus) this.ServerDataStruct.PartyStatusType;
        public bool IsInGame => NetworkState == NetworkStateE.Connected;
        public NetworkStateE NetworkState => (NetworkStateE) this.ServerDataStruct.NetworkState;
        public int Latency => ServerDataStruct.Latency;
        public string Guild => NativeStringReader.ReadString(ServerDataStruct.GuildNameAddress, M);
        public BetrayalData BetrayalData => GetObject<BetrayalData>(M.Read<long>(Address + 0x3C8, 0x718)); // TODO: 3.12.2

        public IList<ushort> SkillBarIds
        {
            get
            {
                if (Address == 0) return new List<ushort>();
                
                var readAddr = _cachedValue.Value.SkillBarIds;

                var res = new List<ushort>
                {
                    readAddr.SkillBar1,
                    readAddr.SkillBar2,
                    readAddr.SkillBar3,
                    readAddr.SkillBar4,
                    readAddr.SkillBar5,
                    readAddr.SkillBar6,
                    readAddr.SkillBar7,
                    readAddr.SkillBar8,
                    readAddr.SkillBar9,
                    readAddr.SkillBar10,
                    readAddr.SkillBar11,
                    readAddr.SkillBar12,
                    readAddr.SkillBar13
                };

                return res;
            }
        }

        public IList<ushort> PassiveSkillIds
        {
            get
            {
                var passiveSkillIds = new List<ushort>();

                if (Address == 0)
                {
                    return passiveSkillIds;
                }

                
                var first = PlayerInformation.AllocatedPassivesIds.First;
                var last = PlayerInformation.AllocatedPassivesIds.Last;
                var totalStats = (int) (last - first);
                
                if (totalStats < 0 || totalStats > 500)
                {
                    return passiveSkillIds;
                }

                var bytes = M.ReadMem(first, totalStats);
                for (var i = 0; i < bytes.Length; i += 2)
                {
                    var id = BitConverter.ToUInt16(bytes, i);
                    passiveSkillIds.Add(id);
                }

                return passiveSkillIds;
            }
        }

        #endregion

        #region Stash Tabs

        public IList<ServerStashTab> PlayerStashTabs => GetStashTabs(ServerDataStruct.PlayerStashTabs);
        public IList<ServerStashTab> GuildStashTabs =>  GetStashTabs(ServerDataStruct.GuildStashTabs);

        private IList<ServerStashTab> GetStashTabs(NativePtrArray tabs)
        {
            var first = tabs.First;
            var last = tabs.Last;

            if (first <= 0 || last <= 0)
            {
                return new List<ServerStashTab>();
            }
            
            var tabCount = (last - first) / ServerStashTab.StructSize;
            
            if (tabCount <= 0 || tabCount > MaxElementsToScan) 
            {
                return new List<ServerStashTab>();
            }

            return M.ReadStructsArray<ServerStashTab>(first, last, ServerStashTab.StructSize, TheGame).ToList();
        }

        #endregion

        #region Inventories

        public IList<InventoryHolder> PlayerInventories => GetInventory(ServerDataStruct.PlayerInventories);
        public IList<InventoryHolder> NPCInventories => GetInventory(ServerDataStruct.NPCInventories);
        public IList<InventoryHolder> GuildInventories => GetInventory(ServerDataStruct.GuildInventories);

        private IList<InventoryHolder> GetInventory(NativePtrArray inventories)
        {
            var first = inventories.First;
            var last = inventories.Last;

            if (first <= 0 || last <= 0)
            {
                return new List<InventoryHolder>();
            }

            var inventoryCount = (last - first) / InventoryHolder.StructSize;

            if (inventoryCount < 0 || inventoryCount > MaxElementsToScan)
            {
                return new List<InventoryHolder>();
            }

            return M.ReadStructsArray<InventoryHolder>(first, last, InventoryHolder.StructSize, this).ToList();
        }

        #region Inventory Util functions

        public ServerInventory GetPlayerInventoryBySlot(InventorySlotE slot)
        {
            foreach (var inventory in PlayerInventories)
            {
                if (inventory.Inventory.InventSlot == slot)
                    return inventory.Inventory;
            }

            return null;
        }

        public ServerInventory GetPlayerInventoryByType(InventoryTypeE type)
        {
            foreach (var inventory in PlayerInventories)
            {
                if (inventory.Inventory.InventType == type)
                    return inventory.Inventory;
            }

            return null;
        }

        public ServerInventory GetPlayerInventoryBySlotAndType(InventoryTypeE type, InventorySlotE slot)
        {
            foreach (var inventory in PlayerInventories)
            {
                if (inventory.Inventory.InventType == type && inventory.Inventory.InventSlot == slot)
                    return inventory.Inventory;
            }

            return null;
        }

        #endregion

        #endregion

        #region Completed Areas

        public IList<WorldArea> CompletedAreas => GetAreas(ServerDataStruct.CompletedMaps);
        public IList<WorldArea> ShapedMaps => new List<WorldArea>();// GetAreas(ServerDataStruct.ShapedAreas);
        public IList<WorldArea> BonusCompletedAreas => GetAreas(ServerDataStruct.BonusCompletedAreas);

        public Dictionary<AtlasRegionE, WorldArea> WatchtowerMaps => GetWatchtowerMaps(Address + ServerDataOffsets.ATLAS_WATCHTOWER_LOCATIONS);

        [ObsoleteAttribute("Elder Guardian Areas were removed with the 3.9.0 Atlas Rework. You should not be using this.", false)]
        public IList<WorldArea> ElderGuardiansAreas => new List<WorldArea>();// GetAreas(ServerDataStruct.ElderGuardiansAreas);

        [ObsoleteAttribute("Masters Areas were removed with the 3.9.0 Atlas Rework. You should not be using this.", false)]
        public IList<WorldArea> MasterAreas => new List<WorldArea>();// GetAreas(ServerDataStruct.MasterAreas);

        [ObsoleteAttribute("Elder Influenced Areas were removed with the 3.9.0 Atlas Rework. You should not be using this.", false)]
        public IList<WorldArea> ShaperElderAreas => new List<WorldArea>();// GetAreas(ServerDataStruct.ElderInfluencedAreas);
        private IList<WorldArea> GetAreas(long address)
        {
            if (Address == 0 || address == 0)
                return new List<WorldArea>();

            var res = new List<WorldArea>();
            var size = M.Read<int>(Address - 0x8);
            var listStart = M.Read<long>(address);
            var error = 0;

            if (listStart == 0 || size == 0)
                return res;

            for (var addr = M.Read<long>(listStart); addr != listStart; addr = M.Read<long>(addr))
            {
                if (addr == 0) return res;
                var byAddress = TheGame.Files.WorldAreas.GetByAddress(M.Read<long>(addr + 0x18));

                if (byAddress != null)
                    res.Add(byAddress);

                if (--size < 0) break;
                error++;

                //Sometimes wrong offsets and read 10000000+ objects
                if (error > 1024)
                {
                    res = new List<WorldArea>();
                    break;
                }
            }

            return res;
        }

        private Dictionary<AtlasRegionE, WorldArea> GetWatchtowerMaps(long first)
        {
            var maps = new Dictionary<AtlasRegionE, WorldArea>();
            if (first == 0)
            {
                return maps;
            }

            first += 0x08; // Array is 8x16 bytes => 8 byte address (Atlas Info) + 8 byte address (Watchtower Map)
            for (int i = 0; i < 8; ++i, first += 0x10)
            {
                var map = ReadObject<WorldArea>(first);
                maps[(AtlasRegionE) i] = map ?? new WorldArea();
            }

            return maps;
        }

        #endregion

        #region Atlas

        public byte GetAtlasRegionUpgradesByRegion(int regionId)
        {
            return M.Read<byte>(Address + ServerDataOffsets.ATLAS_REGION_UPGRADES + regionId);
        }

        public byte GetAtlasRegionUpgradesByRegion(AtlasRegion region)
        {
            return M.Read<byte>(Address + ServerDataOffsets.ATLAS_REGION_UPGRADES + region.Index);
        }

        #endregion
    }
}
