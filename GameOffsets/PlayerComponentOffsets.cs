using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct PlayerComponentOffsets
    {
        [FieldOffset(0x184)] public uint XP;
        [FieldOffset(0x188)] public int Strength;
        [FieldOffset(0x18C)] public int Dexterity;
        [FieldOffset(0x190)] public int Intelligence;
        [FieldOffset(0x194)] public byte AllocatedLootId;
        [FieldOffset(0x198)] public byte PantheonMinor;
        [FieldOffset(0x199)] public byte PantheonMajor;
        [FieldOffset(0x21E)] public byte PropheciesCount;
        [FieldOffset(0x1A3)] public byte Level;
        public long PropheciesOffset => 0x220;
        public long ProphecyLength => 0x4; //prophecy prophecyId(UShort), Skip index(byte), Skip unknown(byte)

        public long TrialPassStatesOffset => 0x2b4; // probably wrong
        public int TrialPassStatesLength => 36;

        // TODO consider removing this:
        [FieldOffset(0x38E)] public byte HideoutLevel; // probably wrong. Is this from the time when hideout could only fit X masters in it?
        public long HideoutWrapperOffset => 0x368; // what is this?
    }
}