using System.Runtime.InteropServices;

namespace GameOffsets
{
	[StructLayout(LayoutKind.Explicit, Pack = 1)]
	public struct SkillUiStateOffsets
	{
		[FieldOffset(0x10)]
		public long CooldownLow;

		[FieldOffset(0x18)]
		public long CooldownHigh;

		[FieldOffset(0x30)]
		public int NumberOfUses;

		[FieldOffset(0x3C)]
		public ushort SkillId;
	}
}