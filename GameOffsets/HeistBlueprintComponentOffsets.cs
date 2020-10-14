using System.Runtime.InteropServices;
using GameOffsets.Native;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct HeistBlueprintComponentOffsets
    {
        public static readonly int WingRecordSize = 0x50;
        public static readonly int JobRecordSize = 0x18;
        public static readonly int RoomRecordSize = 0x18;
        public static readonly int NpcRecordSize = 0x10;

        [FieldOffset(0x8)] public long Owner;
        [FieldOffset(0x1C)] public byte AreaLevel;
        [FieldOffset(0x20)] public NativePtrArray Wings;
    }
}