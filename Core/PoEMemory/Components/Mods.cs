using System;
using System.Collections.Generic;
using System.Linq;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.PoEMemory.Models;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using GameOffsets;
using GameOffsets.Native;

namespace ExileCore.PoEMemory.Components
{
    public class Mods : Component
    {
        private readonly CachedValue<ModsComponentOffsets> _CachedValue;

        public Mods()
        {
            _CachedValue = new FrameCache<ModsComponentOffsets>(() => M.Read<ModsComponentOffsets>(Address));
        }

        public ModsComponentOffsets ModsStruct => _CachedValue.Value;

        public string UniqueName => GetUniqueName(ModsStruct.UniqueName);
        public bool Identified => Address != 0 && ModsStruct.Identified;
        public ItemRarity ItemRarity => Address != 0 ? (ItemRarity) ModsStruct.ItemRarity : ItemRarity.Normal;

        public long Hash => ModsStruct.ImplicitModsArray.GetHashCode() ^ ModsStruct.ExplicitModsArray.GetHashCode() ^
                            ModsStruct.GetHashCode();

        public int ItemLevel => Address != 0 ? ModsStruct.ItemLevel : 1;
        public int RequiredLevel => Address != 0 ? ModsStruct.RequiredLevel : 1;
        public bool IsUsable => Address != 0 && ModsStruct.IsUsable == 1;
        public bool IsMirrored => Address != 0 && ModsStruct.IsMirrored == 1;
        public bool IsSplit => Address != 0 && ModsStruct.IsSplit == 1;
        public string IncubatorName => Address != 0 && ModsStruct.IncubatorKey != 0
            ? M.ReadStringU(M.Read<long>(ModsStruct.IncubatorKey, 0x20)) : null;

        public List<ItemMod> ItemMods
        {
            get
            {
                var implicitMods = GetMods(ModsStruct.ImplicitModsArray);
                var explicitMods = GetMods(ModsStruct.ExplicitModsArray);
                var enchantMods = GetMods(ModsStruct.EnchantedModsArray);
                return implicitMods.Concat(explicitMods).Concat(enchantMods).ToList();
            }
        }
        public ItemStats ItemStats => new ItemStats(Owner);
        public List<string> HumanStats => GetStats(ModsStruct.ExplicitStatsArray);
        public List<string> HumanCraftedStats => GetStats(ModsStruct.CraftedStatsArray);
        public List<string> HumanImpStats => GetStats(ModsStruct.ImplicitStatsArray);
        public List<string> HumanFracturedStats => GetStats(ModsStruct.FracturedStatsArray);
        public List<string> HumanEnchantedStats => GetStats(ModsStruct.EnchantedStatsArray);
        public int FracturedCount => HumanFracturedStats.Count;
        public bool IsFractured => FracturedCount > 0;
        public bool IsSynthesized => ItemMods != null && 
                                     ItemMods.Take(HumanImpStats.Count).Any(x => x.RawName.StartsWith("SynthesisImplicit"));
        public int SynthesizedCount => IsSynthesized ? HumanImpStats.Count : 0;
        public bool IsTalisman => ItemMods != null &&
                                  ItemMods.Take(HumanImpStats.Count).Any(x => x.RawName.StartsWith("Talisman"));
        public int TalismanCount => IsTalisman ? HumanImpStats.Count : 0;
        public int VeiledCount => ItemMods != null ?
                                  ItemMods.Skip(HumanImpStats.Count).Count(x => x.RawName.StartsWith("Veiled")) : 0;
        public bool IsVeiled => VeiledCount > 0;
        
        private string GetUniqueName(NativePtrArray source)
        {
            var words = new List<string>();
            if (Address == 0) return string.Empty;

            for (var first = source.First; first < source.Last; first += ModsComponentOffsets.NameRecordSize)
            {
                words.Add(M.ReadStringU(M.Read<long>(first, ModsComponentOffsets.NameOffset)).Trim());
            }

            return Cache.StringCache.Read($"{nameof(Mods)}{source.First}", () => string.Join(" ", words.ToArray()));
        }

        private List<ItemMod> GetMods(NativePtrArray source)
        {
            var mods = new List<ItemMod>();
            if (Address == 0) return mods;
            if (source.Size / ModsComponentOffsets.ItemModRecordSize > 12) return mods;

            for (var modAddress = source.First;
                modAddress < source.Last;
                modAddress += ModsComponentOffsets.ItemModRecordSize)
            {
                mods.Add(GetObject<ItemMod>(modAddress));
            }

            return mods;
        }

        private List<string> GetStats(NativePtrArray source)
        {
            var stats = new List<string>();
            if (Address == 0) return stats;
            var readPointersArray = M.ReadPointersArray(source.First, source.Last, ModsComponentOffsets.StatRecordSize);

            foreach (var statAddress in readPointersArray)
            {
                stats.Add(Cache.StringCache.Read($"{nameof(Mods)}{statAddress}", () => M.ReadStringU(statAddress)));
            }

            return stats;
        }

        [Obsolete("Use IsFractured", false)]
        public bool HaveFractured => IsFractured;

        [Obsolete("Use FracturedCount", false)]
        public int CountFractured => FracturedCount;
        [Obsolete("Use IsSynthesized", false)]
        public bool Synthesised => IsSynthesized;
    }
}