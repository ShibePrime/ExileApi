using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IngameStateOffsets
    {
        [FieldOffset(0x30)] public long Data;
        [FieldOffset(0xC0)] public long EntityLabelMap;
        [FieldOffset(0x98)] public long IngameUi; 
        [FieldOffset(0x498)] public long ServerData;
        [FieldOffset(0x5C0)] public long UIRoot;
        [FieldOffset(0x608)] public long UIHoverElement;
        [FieldOffset(0x5f8)] public long UIHoverTooltip; 
        [FieldOffset(0x608)] public long UIHover; //telling different hovered objects
        [FieldOffset(0x5C4)] public float UIHoverX;
        [FieldOffset(0x5C8)] public float UIHoverY;
        [FieldOffset(0x5CC)] public float CurentUElementPosX;
        [FieldOffset(0x5D0)] public float CurentUElementPosY;
        [FieldOffset(0x670)] public float TimeInGame; 
        [FieldOffset(0x674)] public float TimeInGameF; 
        [FieldOffset(0x618)] public int DiagnosticInfoType;
        [FieldOffset(0x848)] public long LatencyRectangle;
        [FieldOffset(0xCE8)] public long FrameTimeRectangle;
        [FieldOffset(0xF38)] public long FPSRectangle;
        [FieldOffset(0x788)] public int Camera; 
    }
}
