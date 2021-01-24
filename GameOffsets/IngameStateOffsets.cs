using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IngameStateOffsets
    {
	    [FieldOffset(0x98)] public long IngameUi;
	    [FieldOffset(0xA0)] public long EntityLabelMap;
	    [FieldOffset(0x30)] public long Data;
	    [FieldOffset(0x418)] public long ServerData;
	    [FieldOffset(0x540)] public long UIRoot;
	    [FieldOffset(0x58C)] public long UIHoverTooltip; //if this is a memory region, that tells if ANY tooltip is shown on the screen, then this offset is correct
	    [FieldOffset(0x670)] public float CurentUElementPosX;
	    [FieldOffset(0x674)] public float CurentUElementPosY;
	    [FieldOffset(0x578)] public long UIHover; //if this one is for telling different hovered objects, then offset is correct
	    [FieldOffset(0x5B0)] public int MouseXGlobal;
	    [FieldOffset(0x5B4)] public int MouseYGlobal;
	    [FieldOffset(0x6AC)] public float UIHoverX;
	    [FieldOffset(0x6B0)] public float UIHoverY;
	    [FieldOffset(0x1418)] public float MouseXInGame; //these (mouse X and Y) offsets are actually far before the InGameState memory, but this is also some kind of memory that shows mouse location = same as global.
	    [FieldOffset(0x141C)] public float MouseYInGame;
	    [FieldOffset(0x5EC)] public float TimeInGame;
	    [FieldOffset(0x5F4)] public float TimeInGameF;
	    [FieldOffset(0x6F8)] public int DiagnosticInfoType;
	    [FieldOffset(0x838)] public long LatencyRectangle;
	    [FieldOffset(0xCD8)] public long FrameTimeRectangle;
	    [FieldOffset(0xF28)] public long FPSRectangle;
	    [FieldOffset(0x1080)] public int Camera;
    }
}
