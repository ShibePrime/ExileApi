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
        [FieldOffset(0x17C)] public uint XP;
        [FieldOffset(0x180)] public int Strength;
        [FieldOffset(0x184)] public int Dexterity;
        [FieldOffset(0x188)] public int Intelligence;
        [FieldOffset(0x193)] public byte PantheonMinor;
        [FieldOffset(0x194)] public byte PantheonMajor;
        [FieldOffset(0x1A8)] public byte Level;
        [FieldOffset(0x16C)] public byte AllocatedLootId;                
        [FieldOffset(0x212)] public byte PropheciesCount;
        public long PropheciesOffset => 0x214; // probably wrong
        public long ProphecyLength => 0x4; // probably wrong //prophecy prophecyId(UShort), Skip index(byte), Skip unknown(byte)

        public long TrialPassStatesOffset => 0x2b4; // probably wrong
        public int TrialPassStatesLength => 36;

        // TODO consider removing this:
        [FieldOffset(0x38E)] public byte HideoutLevel; // probably wrong. Is this from the time when hideout could only fit X masters in it?
        public long HideoutWrapperOffset => 0x368; // what is this?
    }
}
