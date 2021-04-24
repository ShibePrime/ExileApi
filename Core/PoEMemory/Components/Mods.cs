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

        public long Hash => ModsStruct.implicitMods.GetHashCode() ^ ModsStruct.explicitMods.GetHashCode() ^
                            ModsStruct.GetHashCode();

        public int ItemLevel => Address != 0 ? ModsStruct.ItemLevel : 1;
        public int RequiredLevel => Address != 0 ? ModsStruct.RequiredLevel : 1;
        public bool IsUsable => Address != 0 && ModsStruct.IsUsable == 1;
        public bool IsMirrored => Address != 0 && ModsStruct.IsMirrored == 1;

        public int FracturedCount => (int) ModsStruct.GetFracturedStats.Size / ModsComponentOffsets.StatRecordSize;
        public bool IsFractured => FracturedCount > 0;
        [Obsolete("Use IsFractured", false)] 
        public bool HaveFractured => IsFractured;

        [Obsolete("Use FracturedCount", false)]
        public int CountFractured => FracturedCount;

        public int SynthesizedCount => (int) ModsStruct.GetSynthesizedStats.Size / ModsComponentOffsets.StatRecordSize;
        public bool IsSynthesized => SynthesizedCount > 0;
        [Obsolete("Use IsSynthesized", false)] 
        public bool Synthesised => IsSynthesized;

        public ItemStats ItemStats => new ItemStats(Owner);
        public List<string> HumanStats => GetStats(ModsStruct.GetStats);
        public List<string> HumanCraftedStats => GetStats(ModsStruct.GetCraftedStats);
        public List<string> HumanImpStats => GetStats(ModsStruct.GetImplicitStats);
        public List<string> FracturedStats => GetStats(ModsStruct.GetFracturedStats);

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

        public List<ItemMod> ItemMods
        {
            get
            {
                var implicitMods = GetMods(ModsStruct.implicitMods);
                var explicitMods = GetMods(ModsStruct.explicitMods);
                var enchantMods = GetMods(ModsStruct.enchantMods);
                return implicitMods.Concat(explicitMods).Concat(enchantMods).ToList();
            }
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
    }
}