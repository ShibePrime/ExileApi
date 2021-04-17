using System.Collections.Generic;
using ExileCore.Shared.Cache;
using GameOffsets;
using JM.LinqFaster;

namespace ExileCore.PoEMemory.Components
{
	public sealed class Buffs : Component
	{
		private readonly CachedValue<List<Buff>> _cachedValueBuffs;

		public Buffs()
		{
			_cachedValueBuffs = new FrameCache<List<Buff>>(ParseBuffs);
		}

		public List<Buff> BuffsList => _cachedValueBuffs.Value;

		public List<Buff> ParseBuffs()
		{
			var buffList = M.Read<BuffsOffsets>(Address);
			var buffPtrs = M.ReadPointersArray(buffList.Buffs.First, buffList.Buffs.Last);
			var result = new List<Buff>(buffPtrs.Count);
			for (int i = 0; i < buffPtrs.Count; i++)
				result.Add(GetObject<Buff>(buffPtrs[i]));
			return result;
		}

		public bool HasBuff(string buff)
		{
			return BuffsList?.AnyF(x => x.Name == buff) ?? false;
		}
	}
}