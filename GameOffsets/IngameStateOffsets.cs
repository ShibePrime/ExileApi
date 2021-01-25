using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IngameStateOffsets
    {
	    [FieldOffset(0x30)] public long Data;
	    [FieldOffset(0x98)] public long IngameUi;
		[FieldOffset(0xB8)] public long EntityLabelMap;
	    [FieldOffset(0x418)] public long ServerData;
	    [FieldOffset(0x540)] public long UIRoot;
	    [FieldOffset(0x578)] public long UIHoverTooltip; //if this is a memory region, that tells if ANY tooltip is shown on the screen, then this offset is correct
	    [FieldOffset(0x580)] public float CurentUElementPosX;
	    [FieldOffset(0x584)] public float CurentUElementPosY;
	    [FieldOffset(0x588)] public long UIHover; //telling different hovered objects
	    [FieldOffset(0x5BC)] public float UIHoverX;
	    [FieldOffset(0x5C0)] public float UIHoverY;
	    [FieldOffset(0x5EC)] public float TimeInGame;
	    [FieldOffset(0x5F4)] public float TimeInGameF;
	    [FieldOffset(0x6F8)] public int DiagnosticInfoType;
	    [FieldOffset(0x838)] public long LatencyRectangle;
	    [FieldOffset(0xCD8)] public long FrameTimeRectangle;
	    [FieldOffset(0xF28)] public long FPSRectangle;
	    [FieldOffset(0x1080)] public int Camera;
    }
}
