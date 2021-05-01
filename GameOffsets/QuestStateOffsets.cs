using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct QuestStateOffsets
    {
        [FieldOffset(0x10)] public long QuestAddress;
        [FieldOffset(0x18)] public long Base; // what is this?
        [FieldOffset(0x20)] public byte QuestStateId;
        [FieldOffset(0x34)] public long QuestStateTextAddress; // wrong, is this still in there?
        [FieldOffset(0x3C)] public long QuestProgressTextAddress; // wrong, is this still in there?
    }
}
