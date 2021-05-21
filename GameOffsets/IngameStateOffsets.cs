using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IngameStateOffsets
    {
        [FieldOffset(0x30)] public long Data;
        [FieldOffset(0xC0)] public long EntityLabelMap;
        [FieldOffset(0xA0)] public long IngameUi;
        [FieldOffset(0x420)] public long ServerData;
        [FieldOffset(0x548)] public long UIRoot;
        [FieldOffset(0x580)] public long UIHoverElement;
        [FieldOffset(0x580)] public long UIHoverTooltip; //if this is a memory region, that tells if ANY tooltip is shown on the screen, then this offset is correct
        [FieldOffset(0x590)] public long UIHover; //telling different hovered objects
        [FieldOffset(0x5C4)] public float UIHoverX;
        [FieldOffset(0x5C8)] public float UIHoverY;
        [FieldOffset(0x5CC)] public float CurentUElementPosX;
        [FieldOffset(0x5D0)] public float CurentUElementPosY;
        [FieldOffset(0x5F4)] public float TimeInGame;
        [FieldOffset(0x5FC)] public float TimeInGameF;
        [FieldOffset(0x618)] public int DiagnosticInfoType;
        [FieldOffset(0x848)] public long LatencyRectangle;
        [FieldOffset(0xCE8)] public long FrameTimeRectangle;
        [FieldOffset(0xF38)] public long FPSRectangle;
        [FieldOffset(0x1080)] public int Camera;    
    }
}
